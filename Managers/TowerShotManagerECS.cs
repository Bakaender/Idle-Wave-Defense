using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Assertions;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EndlessWaveTD
{
    public enum TowerType
    {
        Physical,
        Ice,
        Fire,
        Poison,
        Lightning
    }

    public class TowerShotManagerECS : MonoBehaviour
    {
        public static bool CleanUpBullets;
        private float cleanUpBulletsTimer = 2f;

        private float nextPhysicalFireTime = 0;
        private float nextIceFireTime = 0;
        private float nextFireFireTime = 0;
        private float nextPoisonFireTime = 0;
        private float nextLightningFireTime = 0;

        private Entity physicalPrefabEntity;
        private Entity physicalPrefabNoGraphicsEntity;

        private Entity icePrefabEntity;
        private Entity icePrefabNoGraphicsEntity;

        private Entity firePrefabEntity;
        private Entity firePrefabNoGraphicsEntity;

        private Entity poisonPrefabEntity;
        private Entity poisonPrefabNoGraphicsEntity;

        private Entity lightningPrefabEntity;
        private Entity lightningPrefabNoGraphicsEntity;

        private float TowerRadius = 0.65f;
        private float angleOffset;

        ////Physics stuff to find closest enemy
        //private PointDistanceInput PointDistanceInput;
        //private NativeList<DistanceHit> EnemyDistanceHits;
        //private CollisionFilter BulletEnemyCollisionFilter;

        private float longestTowerRange;
        [HideInInspector] public Vector3 ClosestEnemyPosition;
        [HideInInspector] public float ClosestEnemyDistance;

        private void Awake()
        {
            MainReferences.towerShotManager = this;
            angleOffset = 360 / AllSettings.Instance.MaxTowerProjectiles;
        }

        private void Start()
        {
            physicalPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.physicalTowerManager.PhysicalProjectilePrefab, GameDataManager.instance.Settings);
            physicalPrefabNoGraphicsEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.physicalTowerManager.PhysicalProjectileNoGraphicsPrefab, GameDataManager.instance.Settings);

            icePrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.iceTowerManager.IceProjectilePrefab, GameDataManager.instance.Settings);
            icePrefabNoGraphicsEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.iceTowerManager.IceProjectileNoGraphicsPrefab, GameDataManager.instance.Settings);

            firePrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.fireTowerManager.FireProjectilePrefab, GameDataManager.instance.Settings);
            firePrefabNoGraphicsEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.fireTowerManager.FireProjectileNoGraphicsPrefab, GameDataManager.instance.Settings);

            poisonPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.poisonTowerManager.PoisonProjectilePrefab, GameDataManager.instance.Settings);
            poisonPrefabNoGraphicsEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.poisonTowerManager.PoisonProjectileNoGraphicsPrefab, GameDataManager.instance.Settings);

            lightningPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.lightningTowerManager.LightningProjectilePrefab, GameDataManager.instance.Settings);
            lightningPrefabNoGraphicsEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.lightningTowerManager.LightningProjectileNoGraphicsPrefab, GameDataManager.instance.Settings);

            //EnemyDistanceHits = new NativeList<DistanceHit>(Allocator.Persistent);

            //BulletEnemyCollisionFilter = new CollisionFilter
            //{
            //    BelongsTo = 1 << 1,
            //    CollidesWith = 1
            //};

            //PointDistanceInput = new PointDistanceInput
            //{
            //    Position = Vector3.zero,
            //    MaxDistance = 0,
            //    Filter = BulletEnemyCollisionFilter
            //};
        }

        //private void OnDestroy()
        //{
        //    if (EnemyDistanceHits.IsCreated)
        //    {
        //        EnemyDistanceHits.Dispose();
        //    }
        //}

        private void OnEnable()
        {
            EventManager.StartListening(EventConstants.WaveChanged, NewWave);
            //TODO event for updating longest tower range(call LongestTowerRange) when tower unlocked, or range upgraded.
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventConstants.WaveChanged, NewWave);
        }

        private void Update()
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                cleanUpBulletsTimer -= Time.deltaTime;
                if (cleanUpBulletsTimer <= 0)
                {
                    CleanUpBullets = true;
                    cleanUpBulletsTimer = AllSettings.Instance.CleanUpBulletsDelay;
                }

                nextPhysicalFireTime -= Time.deltaTime;
                nextIceFireTime -= Time.deltaTime;
                nextFireFireTime -= Time.deltaTime;
                nextPoisonFireTime -= Time.deltaTime;
                nextLightningFireTime -= Time.deltaTime;

                //if (nextPhysicalFireTime <= 0 || nextIceFireTime <= 0 || nextFireFireTime <= 0 || nextPoisonFireTime <= 0 || nextLightningFireTime <= 0)
                //{
                //    //TODO after fix LongestTowerRange use the variable instead
                //    GetClosestEnemy(transform.position, LongestTowerRange(), EnemyDistanceHits);
                //    ref PhysicsWorld world = ref GameDataManager.DefaultWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
                //    if (EnemyDistanceHits.IsCreated)
                //    {
                //        //foreach (DistanceHit hit in EnemyDistanceHits.ToArray())
                //        //{
                //        //    Assert.IsTrue(hit.RigidBodyIndex >= 0 && hit.RigidBodyIndex < world.NumBodies);
                //        //    //Assert.IsTrue(math.abs(math.lengthsq(hit.SurfaceNormal) - 1.0f) < 0.01f);

                //        //    ClosestEnemyDistance = math.distance(transform.position, hit.Position);
                //        //    ClosestEnemyPosition = hit.Position;
                //        //}
                //        for (int i = 0; i < EnemyDistanceHits.Length; i++)
                //        {
                //            Assert.IsTrue(EnemyDistanceHits[i].RigidBodyIndex >= 0 && EnemyDistanceHits[i].RigidBodyIndex < world.NumBodies);
                //            ClosestEnemyDistance = math.distance(transform.position, EnemyDistanceHits[i].Position);
                //            ClosestEnemyPosition = EnemyDistanceHits[i].Position;
                //        }
                //    }
                //}

                if (SaveGame.Instance.PhysicalTowerActive && nextPhysicalFireTime <= 0 && MainReferences.physicalTowerManager.AttackRange >= ClosestEnemyDistance)
                {
                    float startingAngle = Vector2.SignedAngle(ClosestEnemyPosition, transform.up);
                    for (int i = 0; i < MainReferences.physicalTowerManager.Projectiles; i++)
                    {
                        SpawnProjectiles(i, startingAngle, TowerType.Physical);
                    }
                    nextPhysicalFireTime = MainReferences.physicalTowerManager.FireDelay;
                }
                if (SaveGame.Instance.IceTowerActive && nextIceFireTime <= 0 && MainReferences.iceTowerManager.AttackRange >= ClosestEnemyDistance)
                {
                    float startingAngle = Vector2.SignedAngle(ClosestEnemyPosition, transform.up);
                    for (int i = 0; i < MainReferences.iceTowerManager.Projectiles; i++)
                    {
                        SpawnProjectiles(i, startingAngle, TowerType.Ice);
                    }
                    nextIceFireTime = MainReferences.iceTowerManager.FireDelay;
                }
                if (SaveGame.Instance.FireTowerActive && nextFireFireTime <= 0 && MainReferences.fireTowerManager.AttackRange >= ClosestEnemyDistance)
                {
                    float startingAngle = Vector2.SignedAngle(ClosestEnemyPosition, transform.up);
                    for (int i = 0; i < MainReferences.fireTowerManager.Projectiles; i++)
                    {
                        SpawnProjectiles(i, startingAngle, TowerType.Fire);
                    }
                    nextFireFireTime = MainReferences.fireTowerManager.FireDelay;
                }
                if (SaveGame.Instance.PoisonTowerActive && nextPoisonFireTime <= 0 && MainReferences.poisonTowerManager.AttackRange >= ClosestEnemyDistance)
                {
                    float startingAngle = Vector2.SignedAngle(ClosestEnemyPosition, transform.up);
                    for (int i = 0; i < MainReferences.poisonTowerManager.Projectiles; i++)
                    {
                        SpawnProjectiles(i, startingAngle, TowerType.Poison);
                    }
                    nextPoisonFireTime = MainReferences.poisonTowerManager.FireDelay;
                }
                if (SaveGame.Instance.LightningTowerActive && nextLightningFireTime <= 0 && MainReferences.lightningTowerManager.AttackRange >= ClosestEnemyDistance)
                {
                    float startingAngle = Vector2.SignedAngle(ClosestEnemyPosition, transform.up);
                    for (int i = 0; i < MainReferences.lightningTowerManager.Projectiles; i++)
                    {
                        SpawnProjectiles(i, startingAngle, TowerType.Lightning);
                    }
                    nextLightningFireTime = MainReferences.lightningTowerManager.FireDelay;
                }
            }
        }

        //[BurstCompile] //Integrator:ParallelIntegrateMotionsJob
        //public struct PointDistanceJob : IJob
        //{
        //    public PointDistanceInput PointDistanceInput;
        //    public NativeList<DistanceHit> DistanceHits;
        //    [ReadOnly] public PhysicsWorld World;

        //    public void Execute()
        //    {
        //        if (World.CalculateDistance(PointDistanceInput, out DistanceHit hit))
        //        {
        //            DistanceHits.Add(hit);
        //        }
        //    }
        //}

        //void GetClosestEnemy(float3 origin, float distance, NativeList<DistanceHit> distanceHits)
        //{
        //    distanceHits.Clear();

        //    ref PhysicsWorld world = ref GameDataManager.DefaultWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;

        //    PointDistanceInput.Position = origin;
        //    PointDistanceInput.MaxDistance = distance;

        //    new PointDistanceJob
        //    {
        //        PointDistanceInput = PointDistanceInput,
        //        DistanceHits = distanceHits,
        //        World = world,
        //    }.Schedule().Complete();
        //}

        private void SpawnProjectiles(int i, float startAngle, TowerType towerType)
        {
            Vector3 pos;

            //Alternate sides for extra projectiles.
            if (i % 2 == 1)
            {
                pos = MathFormulas.RandomCircle(Vector3.zero, TowerRadius, startAngle + angleOffset * ((i + 1) / 2));
            }
            else
            {
                pos = MathFormulas.RandomCircle(Vector3.zero, TowerRadius, startAngle - angleOffset * (i / 2));
            }
            Entity bullet;
            switch (towerType)
            {
                case TowerType.Physical:
                default:
                    if (MainReferences.optionsManager.ShowPhysicalProjectiles)
                    {
                        bullet = GameDataManager.instance.Manager.Instantiate(physicalPrefabEntity);
                    }
                    else
                    {
                        bullet = GameDataManager.instance.Manager.Instantiate(physicalPrefabNoGraphicsEntity);
                    } 
                    break;

                case TowerType.Ice:
                    if (MainReferences.optionsManager.ShowIceProjectiles)
                        bullet = GameDataManager.instance.Manager.Instantiate(icePrefabEntity);
                    else
                        bullet = GameDataManager.instance.Manager.Instantiate(icePrefabNoGraphicsEntity);
                    break;

                case TowerType.Fire:
                    if (MainReferences.optionsManager.ShowFireProjectiles)
                        bullet = GameDataManager.instance.Manager.Instantiate(firePrefabEntity);
                    else
                        bullet = GameDataManager.instance.Manager.Instantiate(firePrefabNoGraphicsEntity);
                    break;

                case TowerType.Poison:
                    if (MainReferences.optionsManager.ShowPoisonProjectiles)
                        bullet = GameDataManager.instance.Manager.Instantiate(poisonPrefabEntity);
                    else
                        bullet = GameDataManager.instance.Manager.Instantiate(poisonPrefabNoGraphicsEntity);
                    break;

                case TowerType.Lightning:
                    if (MainReferences.optionsManager.ShowLightningProjectiles)
                        bullet = GameDataManager.instance.Manager.Instantiate(lightningPrefabEntity);
                    else
                        bullet = GameDataManager.instance.Manager.Instantiate(lightningPrefabNoGraphicsEntity);
                    break;
            }
            
            GameDataManager.instance.Manager.SetComponentData(bullet, new Translation { Value = pos });
            GameDataManager.instance.Manager.SetComponentData(bullet, new BulletData
            {
                BulletType = (int)towerType,
                moveSpeed = AllSettings.Instance.ProjectilMoveSpeed,
                moveDirection = pos
            });
        }

        private void NewWave()
        {
            ClosestEnemyDistance = float.MaxValue;
        }

        ////TODO remove the return once setup to update on events etc.
        //public float LongestTowerRange()
        //{
        //    longestTowerRange = 0;
        //    if (SaveGame.Instance.PhysicalTowerActive && MainReferences.physicalTowerManager.AttackRange > longestTowerRange)
        //    {
        //        longestTowerRange = MainReferences.physicalTowerManager.AttackRange;
        //    }
        //    if (SaveGame.Instance.IceTowerActive && MainReferences.iceTowerManager.AttackRange > longestTowerRange)
        //    {
        //        longestTowerRange = MainReferences.iceTowerManager.AttackRange;
        //    }
        //    if (SaveGame.Instance.FireTowerActive && MainReferences.fireTowerManager.AttackRange > longestTowerRange)
        //    {
        //        longestTowerRange = MainReferences.fireTowerManager.AttackRange;
        //    }
        //    if (SaveGame.Instance.PoisonTowerActive && MainReferences.poisonTowerManager.AttackRange > longestTowerRange)
        //    {
        //        longestTowerRange = MainReferences.poisonTowerManager.AttackRange;
        //    }
        //    if (SaveGame.Instance.LightningTowerActive && MainReferences.lightningTowerManager.AttackRange > longestTowerRange)
        //    {
        //        longestTowerRange = MainReferences.lightningTowerManager.AttackRange;
        //    }

        //    return longestTowerRange;
        //}
    }
}
