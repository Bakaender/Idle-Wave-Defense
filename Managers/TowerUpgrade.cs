using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EndlessWaveTD
{
    [System.Serializable]
    public class TowerUpgrade
    {
        public string UiTitle;
        public float Value;
        public float ScalePercent;
        public bool ConstantUpgrade;
        public float ConstantUpgradeValue;
        public double UpgradeCostDigits;
        public int UpgradeCostExponent;
        public float UpgradeCostScalePercent;
        [HideInInspector] public BigDouble UpgradeCost;
        public TMP_Text[] UiTexts;
    }
}
