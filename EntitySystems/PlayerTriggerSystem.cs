using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using System.ComponentModel;
using Unity.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections.LowLevel.Unsafe;

namespace EndlessWaveTD
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class PlayerTriggerSystem : JobComponentSystem
    {
        BuildPhysicsWorld physicsWorld;
        StepPhysicsWorld stepWorld;

        protected override void OnCreate()
        {
            physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                JobHandle jobHandle = new PlayerTriggerJob
                {
                    PlayerDataGroup = GetComponentDataFromEntity<PlayerData>(true),
                    EnemyGroup = GetComponentDataFromEntity<EnemyData>(),
                    BulletGroup = GetComponentDataFromEntity<BulletData>()
                }.Schedule(stepWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);

                return jobHandle;
            }

            return inputDeps;
        }

        struct PlayerTriggerJob : ITriggerEventsJob
        {
            [Unity.Collections.ReadOnly] public ComponentDataFromEntity<PlayerData> PlayerDataGroup;
            public ComponentDataFromEntity<EnemyData> EnemyGroup;
            public ComponentDataFromEntity<BulletData> BulletGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                bool isAPlayer = PlayerDataGroup.Exists(entityA);
                bool isBPlayer = PlayerDataGroup.Exists(entityB);

                bool isABullet = BulletGroup.Exists(entityA);
                bool isBBullet = BulletGroup.Exists(entityB);

                bool isAEnemy = EnemyGroup.Exists(entityA);
                bool isBEnemy = EnemyGroup.Exists(entityB);

                if ((isAPlayer && isBBullet) || (isBPlayer && isABullet) // Player-Bullet Collision
                    || (isABullet && isBBullet) // Bullet-Bullet Collision
                    || (isAEnemy && isBEnemy)) // Enemy-Enemy Collision
                {
                    return;
                }
                else // Enemy-Player or Enemy-Bullet Collision
                {
                    if (isAEnemy)
                    {
                        EnemyData enemyDat = EnemyGroup[entityA];
                        if (isBBullet) // Enemy-Bullet collision
                        {
                            BulletData bullet = BulletGroup[entityB];
                            bullet.ShouldDestroy = true;
                            bullet.ShouldMove = true;

                            switch (bullet.BulletType)
                            {
                                case (int)TowerType.Physical:
                                    //enemyDat.PhysicalHits++; not needed because bouncer does the hit on spawn.
                                    enemyDat.PhysicalBouncers++;
                                    break;
                                case (int)TowerType.Ice:
                                    enemyDat.IceHits++;
                                    enemyDat.IsSlowed = true;
                                    enemyDat.SlowDuration = MainReferences.iceTowerManager.SlowDuration;
                                    break;
                                case (int)TowerType.Fire:
                                    //enemyDat.FireHits++; not needed because explosion does the hit on spawn.
                                    enemyDat.FireExplosions++;
                                    break;
                                case (int)TowerType.Poison:
                                    enemyDat.PoisonHits++;
                                    if (!enemyDat.IsPoisoned)
                                    {
                                        enemyDat.NextPoisonTick = MainReferences.poisonTowerManager.PoisonTickTime;
                                    }
                                    enemyDat.IsPoisoned = true;
                                    enemyDat.PoisonDuration = MainReferences.poisonTowerManager.PoisonDuration;
                                    break;
                                case (int)TowerType.Lightning:
                                    enemyDat.ChainLightning = true;
                                    enemyDat.LightningHits++;
                                    break;
                            }
                            EnemyGroup[entityA] = enemyDat;

                            BulletGroup[entityB] = bullet;
                        }
                        else if (isBPlayer) // Enemy-Player collision
                        { 
                            enemyDat.ShouldDestroy = true;
                            EnemyGroup[entityA] = enemyDat;
                        }
                    }
                    //TODO implement anyway, copy above but reversed entities.
                    //This never actually runs.. Probably something like the object with the physics body is always first(A)??
                    //else if (isBEnemy)
                    //{
                    //    EnemyData enemyDat = EnemyGroup[entityB];
                    //    if (isABullet) // Enemy-Bullet collision
                    //    {
                    //        BulletData bullet = BulletGroup[entityA];
                    //        bullet.ShouldDestroyImediate = true;
                    //        //switch (bullet.BulletType)
                    //        //{
                    //        //    case (int)TowerType.Physical:
                    //        //        enemyDat.PhysicalHits++;
                    //        //        break;
                    //        //    case (int)TowerType.Ice:
                    //        //        enemyDat.IceHits++;
                    //        //        enemyDat.IsSlowed = true;
                    //        //        //TODO slow duration
                    //        //        //enemyDat.SlowDuration = ???
                    //        //        break;
                    //        //    case (int)TowerType.Fire:
                    //        //        enemyDat.FireHits++;
                    //        //        break;
                    //        //    case (int)TowerType.Poison:
                    //        //        enemyDat.PoisonHits++;
                    //        //        enemyDat.IsPoisoned = true;
                    //        //        //TODO poison duration
                    //        //        //enemyDat.PoisonDuration = ???
                    //        //        break;
                    //        //    case (int)TowerType.Lightning:
                    //        //        enemyDat.LightningHits++;
                    //        //        break;
                    //        //}
                    //        BulletGroup[entityA] = bullet;
                    //    }
                    //    else if (isAPlayer)// Enemy-Player collision
                    //    {
                    //        enemyDat.ShouldDestroy = true;
                    //        EnemyGroup[entityB] = enemyDat;
                    //    }
                    //}
                }
            }
        }
    }
}
