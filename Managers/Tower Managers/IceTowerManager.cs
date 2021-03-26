using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class IceTowerManager : MonoBehaviour
    {
        [Header("Projectile Prefabs")]
        public GameObject IceProjectilePrefab;
        public GameObject IceProjectileNoGraphicsPrefab;

        #region Base Settings for setup and resets(ranking up)
        [Header("Projectiles")]
        public int BaseProjectiles = 1;

        [Header("Damage", order = 1)]
        public double BaseDamageDig = 1;
        public long BaseDamageExp = 0;
        public float DamageScalePercent = 1.1f;

        [Header("Range")]
        public float BaseRange = 5f;

        [Header("Attack Speed")]
        public float BaseFireDelay = 1f;

        [Header("Base Special Stats")]
        public float BaseSlowDuration = 5f;
        public float BaseSlowAmmount = 0.4f;
        #endregion

        #region Modified Settings calculated from base on load/resets(ranking up)
        [HideInInspector] public int Projectiles;
        [HideInInspector] public BigDouble Damage;

        [HideInInspector] public float AttackRange;
        [HideInInspector] public float FireDelay;

        [HideInInspector] public float SlowDuration;
        [HideInInspector] public float SlowAmmount;

        #endregion

        private void Awake()
        {
            MainReferences.iceTowerManager = this;

            if (BaseProjectiles > AllSettings.Instance.MaxTowerProjectiles) BaseProjectiles = AllSettings.Instance.MaxTowerProjectiles;

            #region Copy Base settings to modifiable settings

            Projectiles = BaseProjectiles;
            Damage = new BigDouble(BaseDamageDig, BaseDamageExp);
            AttackRange = BaseRange;
            FireDelay = BaseFireDelay;
            SlowDuration = BaseSlowDuration;
            SlowAmmount = BaseSlowAmmount;

            #endregion         
            //TODO modify based from save
        }
    }
}
