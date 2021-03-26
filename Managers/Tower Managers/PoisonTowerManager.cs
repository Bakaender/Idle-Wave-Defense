using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class PoisonTowerManager : MonoBehaviour
    {
        [Header("Projectile Prefabs")]
        public GameObject PoisonProjectilePrefab;
        public GameObject PoisonExplosionPrefab;
        public GameObject PoisonProjectileNoGraphicsPrefab;

        #region Base Settings for setup and resets(ranking up)
        [Header("Projectiles")]
        public int BaseProjectiles = 1;

        [Header("Damage", order = 1)]
        public double BaseDamageDig = 1;
        public long BaseDamageExp = 0;
        public float DamageScalePercent = 1.1f;
        //PoisonTick
        public double BaseTickDamageDig = 1;
        public long BaseTickDamageExp = 0;
        public float TickDamageScalePercent = 1.1f;

        [Header("Range")]
        public float BaseRange = 5f;

        [Header("Attack Speed")]
        public float BaseFireDelay = 1f;

        [Header("Base Special Stats")]
        public float StartingPoisonDuration = 5f;
        public float StartingPoisonTickTime = 1f;
        public float ExplosionTime = .5f;
        public float StartingExplosionRange = 3f;
        #endregion

        #region Modified Settings calculated from base on load/resets(ranking up)
        [HideInInspector] public int Projectiles;
        [HideInInspector] public BigDouble Damage;
        [HideInInspector] public BigDouble TickDamage;

        [HideInInspector] public float AttackRange;
        [HideInInspector] public float FireDelay;

        [HideInInspector] public float PoisonTickTime;
        [HideInInspector] public float PoisonDuration;
        [HideInInspector] public float ExplosionRange;
        #endregion

        private void Awake()
        {
            MainReferences.poisonTowerManager = this;

            if (BaseProjectiles > AllSettings.Instance.MaxTowerProjectiles) BaseProjectiles = AllSettings.Instance.MaxTowerProjectiles;

            #region Copy Base settings to modifiable settings

            Projectiles = BaseProjectiles;
            Damage = new BigDouble(BaseDamageDig, BaseDamageExp);
            TickDamage = new BigDouble(BaseTickDamageDig, BaseTickDamageExp);
            AttackRange = BaseRange;
            FireDelay = BaseFireDelay;
            PoisonTickTime = StartingPoisonTickTime;
            PoisonDuration = StartingPoisonDuration;
            ExplosionRange = StartingExplosionRange;

            #endregion         
            //TODO modify based from save
        }
    }
}
