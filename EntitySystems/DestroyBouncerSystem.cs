using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

namespace EndlessWaveTD
{
    public class DestroyBouncerSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                Entities.WithoutBurst().WithStructuralChanges()
                .WithName("DestroyBouncerSystem")
                .ForEach((Entity entity, ref PhysicalBouncerData bouncerData) =>
                {
                    if (bouncerData.Destroy)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                }).Run();
            }
            return inputDeps;
        }
    }
}
