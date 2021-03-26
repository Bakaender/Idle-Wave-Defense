using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace EndlessWaveTD
{
    [GenerateAuthoringComponent]
    public struct PhysicalBouncerData : IComponentData
    {
        public Entity FollowEnemy;
        public int Bounces;
        public bool Destroy;
        public bool NeedNewEnemy;
    }
}
