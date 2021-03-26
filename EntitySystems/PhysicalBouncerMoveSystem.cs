using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

namespace EndlessWaveTD
{
    [UpdateBefore(typeof(DestroyEnemySystem))]
    public class PhysicalBouncerMoveSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                EntityQuery enemiesQuery = GameDataManager.instance.Manager.CreateEntityQuery(ComponentType.ReadOnly<EnemyData>());
                NativeArray<Entity> allEnemies = enemiesQuery.ToEntityArray(Allocator.TempJob);
                enemiesQuery.Dispose();
                NativeArray<float3> allEnemiesPositions = new NativeArray<float3>(allEnemies.Length, Allocator.TempJob);
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    allEnemiesPositions[i] = GameDataManager.instance.Manager.GetComponentData<Translation>(allEnemies[i]).Value;
                }
                NativeArray<int> enemyHitFrequency = new NativeArray<int>(allEnemies.Length, Allocator.TempJob);

                float deltaTime = Time.DeltaTime;
                float moveSpeed = AllSettings.Instance.Physical_BouncerMoveSpeed;
                float bounceDistance = MainReferences.physicalTowerManager.BounceRange;
                float hitDistance = MainReferences.physicalTowerManager.BouncerHitDistance;
                bool destroyOnTargetDie = AllSettings.Instance.Physical_DestroyOnTargetDies;
                bool randomBounce = AllSettings.Instance.Physical_RandomBounce;

                var jobHandle = Entities
                        .WithName("PhysicalBouncerMoveSystem")
                        .ForEach((Entity entity, ref Translation position, ref PhysicalBouncerData bouncerData) =>
                        {
                        //Check if target enemy is still alive.
                        int index = -1;
                            for (int i = 0; i < allEnemies.Length; i++)
                            {
                                if (allEnemies[i] == bouncerData.FollowEnemy)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            if (index != -1) //Target enemy still alive
                            {
                                float distance = math.distance(position.Value, allEnemiesPositions[index]);
                                if (distance <= hitDistance) //Hit target
                                {
                                    enemyHitFrequency[index]++;
                                    bouncerData.Bounces--;

                                    if (bouncerData.Bounces > 0) //Find new target
                                    {
                                        bouncerData.NeedNewEnemy = true;
                                        float closestDistance = float.MaxValue;
                                        for (int x = 0; x < allEnemies.Length; x++)
                                        {
                                            if (x == index)
                                            {
                                                continue;
                                            }
                                            float distance2 = math.distance(position.Value, allEnemiesPositions[x]);
                                            if (distance2 < closestDistance)
                                            {
                                                closestDistance = distance2;
                                                if (closestDistance <= bounceDistance)
                                                {
                                                    bouncerData.FollowEnemy = allEnemies[x];
                                                    bouncerData.NeedNewEnemy = false;
                                                    if (randomBounce)
                                                    {
                                                        break;
                                                    }  
                                                }
                                            }
                                        }
                                        if (bouncerData.NeedNewEnemy) //No new enemy in range.
                                        {
                                            bouncerData.Destroy = true;
                                        }
                                    }
                                    else // Out of bounces, destroy.
                                    {
                                        bouncerData.Destroy = true;
                                    }
                                }
                                else //Move toward target
                                {
                                    //consider putting the rotation in here to remove arcing.
                                    position.Value += math.normalize(allEnemiesPositions[index] - position.Value) * moveSpeed * deltaTime;
                                }
                            }
                            else //Target enemy died.
                            {
                                if (destroyOnTargetDie)
                                {
                                    //Destroy if target enemy dies before reaching it.
                                    bouncerData.Destroy = true;
                                }
                                else
                                {
                                    //Find new target if target enemy dies before reaching it.
                                    bouncerData.NeedNewEnemy = true;
                                    float closestDistance = float.MaxValue;
                                    for (int x = 0; x < allEnemies.Length; x++)
                                    {
                                        float distance2 = math.distance(position.Value, allEnemiesPositions[x]);
                                        if (distance2 < closestDistance)
                                        {
                                            closestDistance = distance2;
                                            if (closestDistance <= bounceDistance)
                                            {
                                                bouncerData.FollowEnemy = allEnemies[x];
                                                bouncerData.NeedNewEnemy = false;
                                                if (randomBounce)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (bouncerData.NeedNewEnemy)
                                    {
                                        bouncerData.Destroy = true;
                                    }
                                }
                            }
                        })
                        .Schedule(inputDeps);
                jobHandle.Complete();

                for (int i = 0; i < enemyHitFrequency.Length; i++)
                {
                    if (enemyHitFrequency[i] > 0)
                    {
                        EnemyData enemyData = GameDataManager.instance.Manager.GetComponentData<EnemyData>(allEnemies[i]);
                        enemyData.PhysicalHits += enemyHitFrequency[i];
                        GameDataManager.instance.Manager.SetComponentData(allEnemies[i], enemyData);
                    }
                }

                allEnemies.Dispose();
                allEnemiesPositions.Dispose();
                enemyHitFrequency.Dispose();

                return jobHandle;
            }
            return inputDeps;
        }
    }
}
