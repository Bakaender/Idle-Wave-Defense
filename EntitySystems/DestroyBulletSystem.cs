using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;

namespace EndlessWaveTD
{
    public class DestroyBulletSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                if (TowerShotManagerECS.CleanUpBullets)
                {
                    Entities.WithoutBurst().WithStructuralChanges()
                        .WithName("DestroyBulletSystem")
                        .ForEach((Entity entity, ref BulletData bulletData) =>
                        {
                            if (bulletData.ShouldDestroy)
                            {
                                EntityManager.DestroyEntity(entity);
                            }
                        })
                        .Run();

                    TowerShotManagerECS.CleanUpBullets = false;
                }
            }
            return inputDeps;
        }
    }
}
