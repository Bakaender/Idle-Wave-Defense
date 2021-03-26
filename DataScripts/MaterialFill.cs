using System;
using Unity.Rendering;
using Unity.Entities;
using Unity.Mathematics;

namespace EndlessWaveTD
{
    [Serializable]
    [MaterialProperty("_FillAmmount", MaterialPropertyFormat.Float)]
    public struct MaterialFill : IComponentData
    {
        public float Value;
    }
}
