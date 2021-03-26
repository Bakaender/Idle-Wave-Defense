using System;
using Unity.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

[GenerateAuthoringComponent]
public struct EnemyData : IComponentData
{
    public double HealthNum;
    public long HealthExpo;
    public float moveSpeed;
    public float3 moveDistance;

    public float3 Position;
    public float HpPercent;

    public bool ShouldDestroy;
    public bool ShouldDestroyBulletHit;

    public bool IsSlowed;
    public bool IsPoisoned;

    public float SlowDuration;
    public float PoisonDuration;
    public float NextPoisonTick;

    public int PhysicalHits;
    public int PhysicalBouncers;
    public int IceHits;
    public int FireHits;
    public int FireExplosions;
    public int PoisonHits;
    public int LightningHits;
    //TODO change to int so spawn multiple chains if get hit multiple bullets at once.
    public bool ChainLightning;

    //public int ClosestEnemyID;
    //public float ClosestEnemyDistance;
}



