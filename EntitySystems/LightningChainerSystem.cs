using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

namespace EndlessWaveTD
{
    public class LightningChainerSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                EntityQuery enemiesQuery = GameDataManager.instance.Manager.CreateEntityQuery(ComponentType.ReadOnly<EnemyData>());
                NativeArray<Entity> allEnemies = enemiesQuery.ToEntityArray(Allocator.TempJob);
                enemiesQuery.Dispose();
                NativeArray<float3> allEnemiesPositions = new NativeArray<float3>(allEnemies.Length, Allocator.TempJob);
                NativeArray<int> hitFrequency = new NativeArray<int>(allEnemies.Length, Allocator.TempJob);
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    allEnemiesPositions[i] = GameDataManager.instance.Manager.GetComponentData<Translation>(allEnemies[i]).Value;
                }
                NativeArray<Entity> EnemyChain = new NativeArray<Entity>(AllSettings.Instance.MaxEnemiesPerWave * AllSettings.Instance.MaxEnemiesPerWave, Allocator.TempJob);
                NativeArray<int> ChainLengths = new NativeArray<int>(AllSettings.Instance.MaxEnemiesPerWave, Allocator.TempJob);
                NativeArray<int> CurrentIndex = new NativeArray<int>(1, Allocator.TempJob);
                CurrentIndex[0] = 0;
                float maxDistance = MainReferences.lightningTowerManager.ChainRange;
                int ChainMaxLength = MainReferences.lightningTowerManager.ChainLength;

                var jobHandle = Entities
                    .WithName("LightningChainerSystem")
                    .ForEach((ref Entity entity, ref Translation position, ref LightningChainData chainerData) =>
                    {
                        int Index = 0;
                        for (int i = 0; i < CurrentIndex[0]; i++)
                        {
                            Index += ChainLengths[i];
                        }

                        int chainLength = 0;
                        EnemyChain[Index + chainLength] = entity;

                        float3 StartPosition = position.Value;

                        for (int x = 0; x < ChainMaxLength; x++)
                        {
                            float closestEnemyDistance = float.MaxValue;
                            bool skip;
                            int nextChainEntityIndex = -1;

                            for (int i = 0; i < allEnemies.Length; i++)
                            {
                                skip = false;
                                for (int y = Index; y < Index + chainLength + 1; y++)
                                {
                                    if (allEnemies[i] == EnemyChain[y])
                                    {
                                        skip = true;
                                        break;
                                    }
                                }
                                if (skip)
                                {
                                    continue;
                                }

                                float distance = math.distance(StartPosition, allEnemiesPositions[i]);
                                if (distance < closestEnemyDistance)
                                {
                                    closestEnemyDistance = distance;
                                    if (distance <= maxDistance)
                                    {
                                        nextChainEntityIndex = i;
                                    }
                                }
                            }
                            if (nextChainEntityIndex != -1)
                            {
                                hitFrequency[nextChainEntityIndex]++;

                                chainLength++;
                                EnemyChain[Index + chainLength] = allEnemies[nextChainEntityIndex];

                                StartPosition = allEnemiesPositions[nextChainEntityIndex];
                            }
                            else
                            {
                                break;
                            }
                        }
                        ChainLengths[CurrentIndex[0]] = chainLength + 1;
                        CurrentIndex[0] += 1;
                    })
                    .Schedule(inputDeps);
                jobHandle.Complete();

                for (int i = 0; i < hitFrequency.Length; i++)
                {
                    if (hitFrequency[i] > 0)
                    {
                        EnemyData enemyData = GameDataManager.instance.Manager.GetComponentData<EnemyData>(allEnemies[i]);
                        enemyData.LightningHits += hitFrequency[i];
                        GameDataManager.instance.Manager.SetComponentData(allEnemies[i], enemyData);
                    }
                }

                GameDataManager.instance.Manager.RemoveComponent<LightningChainData>(allEnemies);

                if (MainReferences.optionsManager.ShowLightningChains)
                {
                    int blah = 0;
                    for (int i = 0; i < CurrentIndex[0]; i++)
                    {
                        for (int x = 0; x < ChainLengths[i] - 1; x++)
                        {
                            if (EntityManager.Exists(EnemyChain[blah]) && EntityManager.Exists(EnemyChain[blah + 1]))
                            {
                                float3 start = EntityManager.GetComponentData<Translation>(EnemyChain[blah]).Value;
                                float3 end = EntityManager.GetComponentData<Translation>(EnemyChain[blah + 1]).Value;
                                Entity bolt = EntityManager.Instantiate(MainReferences.towerSpecialManager.LightningBoltGraphicEntityPrefab);
                                float distance = math.distance(start, end);
                                EntityManager.SetComponentData(bolt, new NonUniformScale { Value = new float3(distance * MainReferences.lightningTowerManager.BoltScaleFactor, 0.1f, 1f) });
                                float3 pos = (start + end) / 2;
                                pos.z = -1;
                                EntityManager.SetComponentData(bolt, new Translation { Value = pos });
                                float3 difference = end - start;
                                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                                EntityManager.SetComponentData(bolt, new Rotation { Value = Quaternion.Euler(new Vector3(0, 0, rotationZ)) });
                                EntityManager.SetComponentData(bolt, new TimedDestroyData { LifeTime = MainReferences.lightningTowerManager.BoltDisplayTime });
                                blah++;
                            }
                            else
                            {
                                blah = 0;
                                for (int y = 0; y < i + 1; y++)
                                {
                                    blah += ChainLengths[y];
                                }
                                blah--;
                                break;
                            }
                        }
                        blah++;
                    }
                }

                allEnemies.Dispose();
                allEnemiesPositions.Dispose();
                hitFrequency.Dispose();
                EnemyChain.Dispose();
                ChainLengths.Dispose();
                CurrentIndex.Dispose();

                return jobHandle;
            }
            return inputDeps;
        }
    }
}
