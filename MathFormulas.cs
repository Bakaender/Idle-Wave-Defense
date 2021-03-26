using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public static class MathFormulas
    {
		public static Vector3 RandomCircle(Vector3 center, float radius, float angle)
		{
			Vector3 pos;
			pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
			pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
			pos.z = center.z;
			return pos;
		}

        public static bool PercentChance(float percent)
        {
            float crit = Random.Range(0f, 1f);
            crit *= 100f;
            if (crit < percent)
            {
                return true;
            }
            return false;
        }

        public static BigDouble BulkUpgradeCost(BigDouble nextUpgradeCost, float upgradeScalePercent, int numberToBuy)
        {
            return nextUpgradeCost * ((Mathf.Pow(upgradeScalePercent, numberToBuy) - 1) / (upgradeScalePercent - 1));
        }

        public static float BulkUpgradeValue(float startingValue, float upgradeScalePercent, int numberToUpgrade)
        {
            return startingValue * ((Mathf.Pow(upgradeScalePercent, numberToUpgrade) - 1) / (upgradeScalePercent - 1));
        }

        public static BigDouble GetNextLevelUpgradeCost(BigDouble initialCost, float costScalePercent, int currentOwned)
        {
            //return initialCost * Mathf.Pow(costScalePercent, currentOwned + 1);
            return initialCost * Mathf.Pow(costScalePercent, currentOwned);
        }

        //OPTIMIZE I don't have to have this, can just do 10, 100, 1000 or so.
        //I don't think I can just use log while using my number class.
        //TODO test whats faster initially do some BulkUpgradeCost calls to get around area, or just run up from 1.
        public static int GetMaxUpgradesCanAfford(BigDouble currentMoney, BigDouble nextOneCost, float costScalePercent)
        {
            if (currentMoney < nextOneCost)
            {
                return 0;
            }

            int canBuy = 0;
            while (currentMoney >= nextOneCost)
            {
                currentMoney -= nextOneCost;
                canBuy++;
                nextOneCost *= costScalePercent;
            }

            return canBuy;
        }
    }
}