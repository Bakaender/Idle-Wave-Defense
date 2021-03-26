using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

namespace EndlessWaveTD
{
    public class EnemyMoveSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                float deltaTime = Time.DeltaTime;
                float slowSpeed = MainReferences.iceTowerManager.SlowAmmount;
                NativeArray<float3> closestEnemyPosition = new NativeArray<float3>(1, Allocator.TempJob);
                NativeArray<float> closestEnemyDistance = new NativeArray<float>(1, Allocator.TempJob);
                closestEnemyDistance[0] = float.MaxValue;
                var jobHandle = Entities
                        .WithName("EnemyMoveSystem")
                        .ForEach((ref Translation position, ref EnemyData enemyData) =>
                        {
                            if (!enemyData.IsSlowed)
                            {
                                position.Value -= deltaTime * enemyData.moveSpeed * enemyData.moveDistance;
                                enemyData.Position = position.Value;
                            }
                            else
                            {
                                position.Value -= deltaTime * (enemyData.moveSpeed * slowSpeed) * enemyData.moveDistance;
                                enemyData.Position = position.Value;
                                enemyData.SlowDuration -= deltaTime;
                                if (enemyData.SlowDuration <= 0)
                                {
                                    enemyData.IsSlowed = false;
                                }
                            }

                            float distance = math.distance(float3.zero, position.Value);
                            if (distance < closestEnemyDistance[0])
                            {
                                closestEnemyDistance[0] = distance;
                                closestEnemyPosition[0] = position.Value;
                            }
                        })
                        .Schedule(inputDeps);
                jobHandle.Complete();

                MainReferences.towerShotManager.ClosestEnemyDistance = closestEnemyDistance[0];
                MainReferences.towerShotManager.ClosestEnemyPosition = closestEnemyPosition[0];

                closestEnemyPosition.Dispose();
                closestEnemyDistance.Dispose();

                return jobHandle;
            }
            return inputDeps;
        }
    }
}