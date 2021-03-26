using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

namespace EndlessWaveTD
{
    public class DestroyEnemySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (GameDataManager.gameState == GameState.Playing)
            {
                Entities.WithoutBurst().WithStructuralChanges()
                .WithName("DestroyEnemySystem")
                .ForEach((Entity entity, ref Translation pos, ref EnemyData enemyData) =>
                {
                    if (enemyData.ShouldDestroyBulletHit)
                    {
                        EntityManager.DestroyEntity(entity);
                        EventManager.TriggerEvent(EventConstants.EnemyCountChanged);
                    }
                    else if (enemyData.ShouldDestroy) //Hit Player Lose a life
                    {
                        EntityManager.DestroyEntity(entity);
                        //GameDataManager.instance.LoseLife();
                        EventManager.TriggerEvent(EventConstants.LiveChanged);
                    }
                    else
                    {
                        if (enemyData.PhysicalHits > 0)
                        {
                            while (enemyData.PhysicalHits > 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.PhysicalDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                        enemyData.PhysicalHits--;
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.PhysicalDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                        enemyData.PhysicalHits--;
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                    enemyData.PhysicalHits--;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.PhysicalDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                    enemyData.PhysicalHits--;
                                }
                            }
                        }
                        if (enemyData.PhysicalBouncers > 0)
                        {
                            while (enemyData.PhysicalBouncers > 0)
                            {
                                MainReferences.towerSpecialManager.PhysBounSpawnEnt.Add(entity);
                                MainReferences.towerSpecialManager.PhysBounSpawnPos.Add(pos.Value);
                                enemyData.PhysicalBouncers--;
                            }
                        }

                        if (enemyData.IceHits > 0)
                        {
                            while (enemyData.IceHits > 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.IceDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.IceDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.IceDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                enemyData.IceHits--;
                            }
                        }

                        if (enemyData.FireHits > 0)
                        {
                            while (enemyData.FireHits > 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.FireDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                        enemyData.FireHits--;
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.FireDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                        enemyData.FireHits--;
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                    enemyData.FireHits--;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.FireDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                    enemyData.FireHits--;
                                }
                            }
                        }
                        if (enemyData.FireExplosions > 0)
                        {
                            while (enemyData.FireExplosions > 0)
                            {
                                MainReferences.towerSpecialManager.FireExplosionSpawnPos.Add(pos.Value);
                                enemyData.FireExplosions--;
                            }
                        }

                        if (enemyData.PoisonHits > 0)
                        {
                            while (enemyData.PoisonHits > 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.PoisonDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.PoisonDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.PoisonDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                enemyData.PoisonHits--;
                            }
                        }

                        if (enemyData.LightningHits > 0)
                        {
                            while (enemyData.LightningHits > 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.LightningDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                        enemyData.LightningHits--;
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.LightningDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                        enemyData.LightningHits--;
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                    enemyData.LightningHits--;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.LightningDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                    enemyData.LightningHits--;
                                }
                            }
                        }

                        if (enemyData.IsPoisoned)
                        {
                            enemyData.PoisonDuration -= Time.DeltaTime;
                            enemyData.NextPoisonTick -= Time.DeltaTime;

                            if (enemyData.NextPoisonTick <= 0)
                            {
                                long shiftDigits = enemyData.HealthExpo - MainReferences.damageManager.PoisonTickDamage.Exponent;
                                if (shiftDigits > 0)
                                {
                                    if (shiftDigits >= GameDataManager.EnemyHpDigitsOfPrecision)
                                    {
                                    }
                                    else
                                    {
                                        double newHP = MainReferences.damageManager.PoisonTickDamage.Number;
                                        for (int i = 0; i < shiftDigits; i++)
                                        {
                                            newHP /= 10;
                                        }
                                        enemyData.HealthNum -= newHP;

                                        if (enemyData.HealthNum < 1)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                else if (shiftDigits < 0)
                                {
                                    enemyData.HealthNum = -1;
                                    enemyData.HealthExpo = 0;
                                }
                                else
                                {
                                    enemyData.HealthNum -= MainReferences.damageManager.PoisonTickDamage.Number;
                                    if (enemyData.HealthNum <= 0)
                                    {
                                        enemyData.HealthNum = -1;
                                        enemyData.HealthExpo = 0;
                                    }
                                    else if (enemyData.HealthNum < 1)
                                    {
                                        if (enemyData.HealthExpo > 0)
                                        {
                                            enemyData.HealthNum *= 10;
                                            enemyData.HealthExpo--;
                                        }
                                    }
                                }
                                enemyData.NextPoisonTick = MainReferences.poisonTowerManager.PoisonTickTime;
                            }

                            if (enemyData.PoisonDuration <= 0)
                            {
                                enemyData.IsPoisoned = false;
                            }
                        }

                        //TODO fix, if multiple bullets hit at once, only 1 chain.
                        if (enemyData.ChainLightning)
                        {
                            GameDataManager.instance.Manager.AddComponent<LightningChainData>(entity);
                            enemyData.ChainLightning = false;
                        }

                        if (enemyData.HealthNum <= 0)
                        {
                            if (enemyData.IsPoisoned)
                            {
                                MainReferences.towerSpecialManager.PoisonExplosionSpawnPos.Add(pos.Value);
                            }
                            enemyData.ShouldDestroyBulletHit = true;
                        }
                        else
                        {
                            float test = (float)(enemyData.HealthNum / WaveSpawnerECS.currentWaveEnemyHp.Number);
                            int testExp = (int)(enemyData.HealthExpo - WaveSpawnerECS.currentWaveEnemyHp.Exponent);

                            while (testExp < 0)
                            {
                                test /= 10;
                                testExp++;
                            }
                            enemyData.HpPercent = test;
                        }
                    }
                })
                .Run();
            }
            return inputDeps;
        }
    }
}
