using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace EndlessWaveTD
{
    [GenerateAuthoringComponent]
    public struct TimedDestroyData : IComponentData
    {
        public float LifeTime;
    }
}
