using System;
using Unity.Rendering;
using Unity.Entities;
using Unity.Mathematics;

namespace EndlessWaveTD
{
    [Serializable]
    [MaterialProperty("_ColorRGBA", MaterialPropertyFormat.Float4)]
    public struct MyMaterialColor : IComponentData
    {
        public float4 Value;
    }
}
