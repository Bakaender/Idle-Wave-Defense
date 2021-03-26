using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EndlessWaveTD
{
    [System.Serializable]
    public class GeneralUpgrade : TowerUpgrade
    {
        public GeneralUpgradesEnum UpgradeName;
    }

    /// <summary>
    /// If add to this need to also add to switches for buttons etc.
    /// </summary>
    public enum GeneralUpgradesEnum
    {
        OverallDamage, 
        OverallRange, 
        OverallCritChance, 
        OverallCritDamage, 
        StartingLives, 
        StartingWave, 
        ExpDrop
    }

    public class GeneralUpgrades : MonoBehaviour
    {
        //public GameObject UiPanel;
        //public GameObject UiUpgradePrefab;

        //public GeneralUpgrade[] GeneralUpgradesArray = new GeneralUpgrade[System.Enum.GetNames(typeof(GeneralUpgradesEnum)).Length];

        //public int currentLives;

        //private void OnValidate()
        //{
        //    if (GeneralUpgradesArray.Length != System.Enum.GetNames(typeof(GeneralUpgradesEnum)).Length)
        //    {
        //        System.Array.Resize(ref GeneralUpgradesArray, System.Enum.GetNames(typeof(GeneralUpgradesEnum)).Length);
        //    }

        //    for (int i = 0; i < GeneralUpgradesArray.Length; i++)
        //    {
        //        if ((int)GeneralUpgradesArray[i].UpgradeName != i)
        //        {
        //            GeneralUpgradesArray[i].UpgradeName = (GeneralUpgradesEnum)i;
        //        }
        //    }        
        //}

        //private void Awake()
        //{
        //    MainReferences.generalUpgradesManager = this;

        //    for (int i = 0; i < GeneralUpgradesArray.Length; i++)
        //    {
        //        GeneralUpgradesArray[i].UpgradeCost = new BigDouble(GeneralUpgradesArray[i].UpgradeCostDigits, GeneralUpgradesArray[i].UpgradeCostExponent);
        //        GeneralUpgradesArray[i].UpgradeCost *= Mathf.Pow(GeneralUpgradesArray[i].UpgradeCostScalePercent, SaveGame.Instance.GeneralUpgradeLevels[i]);

        //        if (GeneralUpgradesArray[i].ConstantUpgrade)
        //        {
        //            GeneralUpgradesArray[i].Value += SaveGame.Instance.GeneralUpgradeLevels[i] * GeneralUpgradesArray[i].ConstantUpgradeValue;
        //        }
        //        else
        //        {
        //            GeneralUpgradesArray[i].Value *= Mathf.Pow(GeneralUpgradesArray[i].ScalePercent, SaveGame.Instance.GeneralUpgradeLevels[i]);
        //        }

        //        GameObject uiUpgradeObject = Instantiate(UiUpgradePrefab, UiPanel.transform);
        //        GeneralUpgradesArray[i].UiTexts = uiUpgradeObject.GetComponentsInChildren<TMP_Text>();

        //        GeneralUpgradeBuyButton[] buyButtons = uiUpgradeObject.GetComponentsInChildren<GeneralUpgradeBuyButton>();
        //        if (buyButtons.Length != 4)
        //        {
        //            Debug.LogError("Wrong number of upgrade buttons on prefab. Expected 4, found " + buyButtons.Length);
        //        }
        //        else
        //        {
        //            buyButtons[0].Initialize(GeneralUpgradesArray[i].UpgradeName, 1);
        //            buyButtons[1].Initialize(GeneralUpgradesArray[i].UpgradeName, 10);
        //            buyButtons[2].Initialize(GeneralUpgradesArray[i].UpgradeName, 100);
        //            buyButtons[3].Initialize(GeneralUpgradesArray[i].UpgradeName, 0);
        //        }
        //    }

        //    currentLives = DevelopmentParamaters.StartingLives + (int)GeneralUpgradesArray[(int)GeneralUpgradesEnum.StartingLives].Value;
        //}

        //private void Start()
        //{
        //    for (int i = 0; i < GeneralUpgradesArray.Length; i++)
        //    {
        //        GeneralUpgradesArray[i].UiTexts[0].text = GeneralUpgradesArray[i].UiTitle;
        //    }
        //    UpdateGeneralUpgradesUI();
        //}

        //private void OnEnable()
        //{
        //    EventManager.StartListening(EventConstants.UpdateGeneralUpgradesUi, UpdateGeneralUpgradesUI);
        //}

        //private void OnDisable()
        //{
        //    EventManager.StopListening(EventConstants.UpdateGeneralUpgradesUi, UpdateGeneralUpgradesUI);
        //}

        //public void LoseALife()
        //{
        //    currentLives--;
        //    EventManager.TriggerEvent(EventConstants.LiveChanged);

        //    if (currentLives <= 0)
        //    {
        //        //TODO restart.
        //        Debug.Log("Game Over");
        //    }
        //}

        ///// <summary>
        ///// UiTexts -> 0 = Title, 1 = Level, 2 = Bonus, 3 = Next Bonus, 4 = Upgrade Cost.
        ///// </summary>
        //private void UpdateGeneralUpgradesUI()
        //{
        //    for (int i = 0; i < GeneralUpgradesArray.Length; i++)
        //    {
        //        GeneralUpgradesArray[i].UiTexts[1].text = "Current Level " + (SaveGame.Instance.GeneralUpgradeLevels[i]);

        //        switch (GeneralUpgradesArray[i].UpgradeName)
        //        {
        //            case GeneralUpgradesEnum.OverallDamage:
        //            case GeneralUpgradesEnum.OverallCritDamage:
        //                GeneralUpgradesArray[i].UiTexts[2].text = "Current Bonus " + GeneralUpgradesArray[i].Value * 100 + "%";
        //                GeneralUpgradesArray[i].UiTexts[3].text = "Next Level " + (GeneralUpgradesArray[i].Value * GeneralUpgradesArray[i].ScalePercent) * 100 + "%";
        //                break;

        //            case GeneralUpgradesEnum.OverallRange:
        //            case GeneralUpgradesEnum.OverallCritChance:
        //                GeneralUpgradesArray[i].UiTexts[2].text = "Current Bonus " + GeneralUpgradesArray[i].Value * 100 + "%";
        //                GeneralUpgradesArray[i].UiTexts[3].text = "Next Level " + (GeneralUpgradesArray[i].Value + GeneralUpgradesArray[i].ConstantUpgradeValue) * 100 + "%";
        //                break;
              
        //            case GeneralUpgradesEnum.ExpDrop:
        //                GeneralUpgradesArray[i].UiTexts[2].text = "Current Bonus " + GeneralUpgradesArray[i].Value + "%";
        //                GeneralUpgradesArray[i].UiTexts[3].text = "Next Level " + (GeneralUpgradesArray[i].Value + GeneralUpgradesArray[i].ConstantUpgradeValue) + "%";
        //                break;

        //            case GeneralUpgradesEnum.StartingLives:
        //            case GeneralUpgradesEnum.StartingWave:
        //                GeneralUpgradesArray[i].UiTexts[2].text = "Current Bonus " + GeneralUpgradesArray[i].Value;
        //                GeneralUpgradesArray[i].UiTexts[3].text = "Next Level " + (GeneralUpgradesArray[i].Value + GeneralUpgradesArray[i].ConstantUpgradeValue);
        //                break;

        //            default:
        //                break;
        //        }

        //        GeneralUpgradesArray[i].UiTexts[4].text = "Upgrade Cost " + GeneralUpgradesArray[i].UpgradeCost.ToString();
        //    }
        //}

        ///// <summary>
        ///// Handles buying upgrades.
        ///// </summary>
        ///// <param name="id">What Upgrade to Buy.</param>
        ///// <param name="numberUpgradesToBuy">To buy max upgrades set to 0.</param>
        //public void ButtonClickBuyUpgrade(GeneralUpgradesEnum id, int numberUpgradesToBuy = 1)
        //{

        //    if (numberUpgradesToBuy == 1)
        //    {
        //        SaveGame.Instance.BuyGeneralUpgrade(GeneralUpgradesArray[(int)id].UpgradeCost, id, numberUpgradesToBuy);
        //    }
        //    else if (numberUpgradesToBuy == 10)
        //    {
        //        //TODO cost seems to be wrong.
        //        BigDouble costForTen = MathFormulas.BulkUpgradeCost(GeneralUpgradesArray[(int)id].UpgradeCost, GeneralUpgradesArray[(int)id].UpgradeCostScalePercent, numberUpgradesToBuy);
        //        SaveGame.Instance.BuyGeneralUpgrade(costForTen, id, numberUpgradesToBuy);
        //    }
        //    else if (numberUpgradesToBuy == 100)
        //    {
        //        BigDouble costForHundred = MathFormulas.BulkUpgradeCost(GeneralUpgradesArray[(int)id].UpgradeCost, GeneralUpgradesArray[(int)id].UpgradeCostScalePercent, numberUpgradesToBuy);
        //        SaveGame.Instance.BuyGeneralUpgrade(costForHundred, id, numberUpgradesToBuy);
        //    }
        //    else if (numberUpgradesToBuy == 0)
        //    {
        //        int canAfford = MathFormulas.GetMaxUpgradesCanAfford(SaveGame.Instance.PlayerExp, GeneralUpgradesArray[(int)id].UpgradeCost, GeneralUpgradesArray[(int)id].UpgradeCostScalePercent);
        //        BigDouble costForMax = MathFormulas.BulkUpgradeCost(GeneralUpgradesArray[(int)id].UpgradeCost, GeneralUpgradesArray[(int)id].UpgradeCostScalePercent, canAfford);
        //        SaveGame.Instance.BuyGeneralUpgrade(costForMax, id, canAfford);
        //    }
        //}
    }
}