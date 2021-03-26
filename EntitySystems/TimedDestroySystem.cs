using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

namespace EndlessWaveTD
{
    public class TimedDestroySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                float dt = Time.DeltaTime;
                Entities.WithoutBurst().WithStructuralChanges()
                    .ForEach((Entity entity, ref TimedDestroyData lifetimeData) =>
                    {
                        lifetimeData.LifeTime -= dt;
                        if (lifetimeData.LifeTime <= 0f)
                            EntityManager.DestroyEntity(entity);
                    })
                .Run();  
            }

            return inputDeps;
        }
    }
}
