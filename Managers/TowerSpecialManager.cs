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
using System.Linq;
using System.Collections.Generic;

namespace EndlessWaveTD
{
    public class TowerSpecialManager : MonoBehaviour
    {
        [HideInInspector] public List<Entity> PhysBounSpawnEnt = new List<Entity>();
        [HideInInspector] public List<float3> PhysBounSpawnPos = new List<float3>();

        [HideInInspector] public List<float3> FireExplosionSpawnPos = new List<float3>();

        [HideInInspector] public List<float3> PoisonExplosionSpawnPos = new List<float3>();

        public Entity LightningBoltGraphicEntityPrefab;
        
        public Entity PhysicalBouncerEntityPrefab;
        public Entity PhysicalBounderNoGraphicsEntityPrefab;

        //Physics stuff to find closest enemy
        private PointDistanceInput PointDistanceInput;
        private NativeList<DistanceHit> EnemyDistanceHits;
        private CollisionFilter BulletEnemyCollisionFilter;

        private void Awake()
        {
            MainReferences.towerSpecialManager = this;
        }

        private void Start()
        {
            LightningBoltGraphicEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.lightningTowerManager.LightningBoltGraphicPrefab, GameDataManager.instance.Settings);

            PhysicalBouncerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.physicalTowerManager.PhysicalBouncerPrefab, GameDataManager.instance.Settings);
            PhysicalBounderNoGraphicsEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(MainReferences.physicalTowerManager.PhysicalBouncerNoGraphicsPrefab, GameDataManager.instance.Settings);

            EnemyDistanceHits = new NativeList<DistanceHit>(Allocator.Persistent);

            BulletEnemyCollisionFilter = new CollisionFilter
            {
                BelongsTo = 1 << 1,
                CollidesWith = 1
            };

            PointDistanceInput = new PointDistanceInput
            {
                Position = Vector3.zero,
                MaxDistance = 0,
                Filter = BulletEnemyCollisionFilter
            };
        }

        private void OnDestroy()
        {
            if (EnemyDistanceHits.IsCreated)
            {
                EnemyDistanceHits.Dispose();
            }
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventConstants.Reset, WaveReset);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventConstants.Reset, WaveReset);
        }

        private void SpawnBouncer()
        {
            Entity bouncer;
            if (MainReferences.optionsManager.ShowPhysicalBouncers)
            {
                bouncer = GameDataManager.instance.Manager.Instantiate(PhysicalBouncerEntityPrefab);
            }
            else
            {
                bouncer = GameDataManager.instance.Manager.Instantiate(PhysicalBounderNoGraphicsEntityPrefab);
            }
            GameDataManager.instance.Manager.SetComponentData(bouncer, new PhysicalBouncerData { FollowEnemy = PhysBounSpawnEnt[0], Bounces = MainReferences.physicalTowerManager.NumberOfBounces + 1 });
            GameDataManager.instance.Manager.SetComponentData(bouncer, new Translation { Value = PhysBounSpawnPos[0] });
            PhysBounSpawnEnt.RemoveAt(0);
            PhysBounSpawnPos.RemoveAt(0);
        }

        private void SpawnFireExplsion()
        {   
            if (MainReferences.optionsManager.ShowFireExplosions)
            {
                Instantiate(MainReferences.fireTowerManager.FireExplosionPrefab, FireExplosionSpawnPos[0], Quaternion.identity);
            }

            GetExplosionEnemies(FireExplosionSpawnPos[0], MainReferences.fireTowerManager.ExplosionRange);

            ref PhysicsWorld world = ref GameDataManager.DefaultWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
            if (EnemyDistanceHits.IsCreated)
            {
                for (int i = 0; i < EnemyDistanceHits.Length; i++)
                {
                    Assert.IsTrue(EnemyDistanceHits[i].RigidBodyIndex >= 0 && EnemyDistanceHits[i].RigidBodyIndex < world.NumBodies);
                    EnemyData enemyDat = GameDataManager.instance.Manager.GetComponentData<EnemyData>(EnemyDistanceHits[i].Entity);
                    enemyDat.FireHits++;
                    GameDataManager.instance.Manager.SetComponentData(EnemyDistanceHits[i].Entity, enemyDat);
                }
            }

            FireExplosionSpawnPos.RemoveAt(0);
        }

        private void SpawnPoisonExplosion()
        {
            if (MainReferences.optionsManager.ShowPoisonExplosions)
            {
                Instantiate(MainReferences.poisonTowerManager.PoisonExplosionPrefab, PoisonExplosionSpawnPos[0], Quaternion.identity);
            }

            GetExplosionEnemies(PoisonExplosionSpawnPos[0], MainReferences.poisonTowerManager.ExplosionRange);

            ref PhysicsWorld world = ref GameDataManager.DefaultWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
            if (EnemyDistanceHits.IsCreated)
            {
                for (int i = 0; i < EnemyDistanceHits.Length; i++)
                {
                    Assert.IsTrue(EnemyDistanceHits[i].RigidBodyIndex >= 0 && EnemyDistanceHits[i].RigidBodyIndex < world.NumBodies);
                    EnemyData enemyDat = GameDataManager.instance.Manager.GetComponentData<EnemyData>(EnemyDistanceHits[i].Entity);                    
                    if (!enemyDat.IsPoisoned)
                    {
                        enemyDat.NextPoisonTick = MainReferences.poisonTowerManager.PoisonTickTime;
                    }
                    enemyDat.PoisonHits++;
                    enemyDat.IsPoisoned = true;
                    enemyDat.PoisonDuration = MainReferences.poisonTowerManager.PoisonDuration;
                    GameDataManager.instance.Manager.SetComponentData(EnemyDistanceHits[i].Entity, enemyDat);
                }
            }

            PoisonExplosionSpawnPos.RemoveAt(0);
        }

        private void Update()
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                while (PhysBounSpawnEnt.Count > 0)
                {
                    SpawnBouncer();
                }

                while (FireExplosionSpawnPos.Count > 0)
                {
                    SpawnFireExplsion();
                }

                while (PoisonExplosionSpawnPos.Count > 0)
                {
                    SpawnPoisonExplosion();
                }
            }
        }

        private void WaveReset()
        {
            PhysBounSpawnEnt.Clear();
            PhysBounSpawnPos.Clear();
            FireExplosionSpawnPos.Clear();
            PoisonExplosionSpawnPos.Clear();
        }

        void GetExplosionEnemies(float3 origin, float distance)
        {
            EnemyDistanceHits.Clear();

            ref PhysicsWorld world = ref GameDataManager.DefaultWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;

            PointDistanceInput.Position = origin;
            PointDistanceInput.MaxDistance = distance;

            new PointDistanceJob
            {
                PointDistanceInput = PointDistanceInput,
                DistanceHits = EnemyDistanceHits,
                World = world,
            }.Schedule().Complete();
        }

        [BurstCompile]
        public struct PointDistanceJob : IJob
        {
            public PointDistanceInput PointDistanceInput;
            public NativeList<DistanceHit> DistanceHits;
            [ReadOnly] public PhysicsWorld World;

            public void Execute()
            {
                World.CalculateDistance(PointDistanceInput, ref DistanceHits);
            }
        }
    }
}
