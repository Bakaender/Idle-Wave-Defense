using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

namespace EndlessWaveTD
{
    public class BulletMoveSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                float deltaTime = Time.DeltaTime;
                float gameEdge = AllSettings.Instance.GameAreaEdgeDistance;
                float3 moveToPos = new float3(100, 100, 0);
                var jobHandle = Entities
                        .WithName("BulletMoveSystem")
                        .ForEach((ref Translation position, ref BulletData bulletData) =>
                        {
                        //I could add a component instead of keep moving and setting destroy every frame if performance needs it.
                        if (bulletData.ShouldMove)
                            {
                                position.Value = moveToPos;
                            }
                            else
                            {
                                position.Value += deltaTime * bulletData.moveSpeed * bulletData.moveDirection;
                            }

                            if (math.abs(position.Value.x) >= gameEdge || math.abs(position.Value.y) >= gameEdge)
                            {
                                bulletData.ShouldDestroy = true;
                            }
                        })
                        .Schedule(inputDeps);

                return jobHandle;
            }
            return inputDeps;
        }
    }
}
