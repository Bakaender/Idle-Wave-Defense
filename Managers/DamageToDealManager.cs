using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public class DamageToDealManager : MonoBehaviour
    {
        [HideInInspector] public BigDouble PhysicalDamage;

        [HideInInspector] public BigDouble LightningDamage;

        [HideInInspector] public BigDouble FireDamage;

        [HideInInspector] public BigDouble PoisonDamage;
        [HideInInspector] public BigDouble PoisonTickDamage;

        [HideInInspector] public BigDouble IceDamage;

        private void Awake()
        {
            MainReferences.damageManager = this;
        }

        //Delay 1 frame to make sure all other starts run first.
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            UpdateTowerDamages();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventConstants.UpdateTowersDamage, UpdateTowerDamages);
            //TODO All types of temp bonuses. Range, Crit, etc.
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventConstants.UpdateTowersDamage, UpdateTowerDamages);
        }

        private void UpdateTowerDamages()
        {
            //TODO multiply all temporary bonuses here.
            PhysicalDamage = MainReferences.physicalTowerManager.Damage;

            LightningDamage = MainReferences.lightningTowerManager.Damage;

            FireDamage = MainReferences.fireTowerManager.Damage;

            PoisonDamage = MainReferences.poisonTowerManager.Damage;
            PoisonTickDamage = MainReferences.poisonTowerManager.TickDamage;

            IceDamage = MainReferences.iceTowerManager.Damage;
        }
    }
}
