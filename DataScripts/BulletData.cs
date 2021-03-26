using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public int BulletType;
    public float moveSpeed;
    public float3 moveDirection;
    public bool ShouldDestroy;
    public bool ShouldMove;
}
