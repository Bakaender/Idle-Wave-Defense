using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class PhysicalTowerManager : MonoBehaviour
    {
        [Header("Projectile Prefabs")]
        public GameObject PhysicalProjectilePrefab;
        public GameObject PhysicalBouncerPrefab;

        public GameObject PhysicalProjectileNoGraphicsPrefab;
        public GameObject PhysicalBouncerNoGraphicsPrefab;

        //How close to target before considered hit target.
        [HideInInspector] public float BouncerHitDistance = 0.15f;


        #region Modified Settings calculated from base on load/resets(ranking up)
        [HideInInspector] public int Projectiles;
        [HideInInspector] public BigDouble Damage;

        [HideInInspector] public float AttackRange;
        [HideInInspector] public float FireDelay;

        [HideInInspector] public int NumberOfBounces;
        [HideInInspector] public float BounceRange;
        #endregion

        private void Awake()
        {
            MainReferences.physicalTowerManager = this;

            LoadSettings();
        }

        private void LoadSettings()
        {
            if (AllSettings.Instance.Physical_BaseProjectiles > AllSettings.Instance.MaxTowerProjectiles) AllSettings.Instance.Physical_BaseProjectiles = AllSettings.Instance.MaxTowerProjectiles;

            Projectiles = AllSettings.Instance.Physical_BaseProjectiles;
            Damage = new BigDouble(AllSettings.Instance.Physical_BaseDamageDig, AllSettings.Instance.Physical_BaseDamageExp);
            AttackRange = AllSettings.Instance.Physical_BaseRange;
            FireDelay = AllSettings.Instance.Physical_BaseFireDelay;
            NumberOfBounces = AllSettings.Instance.Physical_StartingNumberOfBounces;
            BounceRange = AllSettings.Instance.Physical_StartingBounceRange;

            //TODO modify based from save
        }
    }
}
