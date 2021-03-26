using System.IO;
using UnityEngine;

namespace EndlessWaveTD
{
    public class GameBalanceSettings
    {
        #region Game Settings
        public int MaxEnemiesPerWave = 250;
        public int StartingExp = 100;
        public int StartingRankPoints = 10;
        public int StartingPremiumCurrency = 300;
        public int StartingLives = 10;
        public int MaxTowerProjectiles = 90;
        public float GameAreaEdgeDistance = 11f;

        public float GameOverResetTime = 1f;
        public float ReviveResetTime = 1f;
        public int MinimumReviveWave = 10;

        public float CleanUpBulletsDelay = 2f;
        public float ProjectilMoveSpeed = 1f;
        #endregion

        #region Base Physical Tower Settings
        public int Physical_BaseProjectiles = 15;
        public double Physical_BaseDamageDig = 10;
        public long Physical_BaseDamageExp = 0;
        public float Physical_DamageScalePercent = 1.1f;
        public float Physical_BaseRange = 15f;
        public float Physical_BaseFireDelay = 0.5f;
        public bool Physical_DestroyOnTargetDies = true;
        public bool Physical_RandomBounce = true;
        public float Physical_BouncerMoveSpeed = 4f;
        public int Physical_StartingNumberOfBounces = 50;
        public float Physical_StartingBounceRange = 4f;
        #endregion

        public float FireTowerUnlockCost = 1;
        public float IceTowerUnlockCost = 1;
        public float LightningTowerUnlockCost = 1;
        public float PoisonTowerUnlockCost = 1;
    }

    public class AllSettings
    {
        //static bool LoadJSON = false;
        //static string SettingsFileName = Application.persistentDataPath +  "/GameBalanceSettings.txt";

        static protected GameBalanceSettings instance;
        public static GameBalanceSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    Create();
                }
                return instance;
            }
        }

        //private static void LoadJSONSettings()
        //{
        //    string optionsJson = (File.Exists(SettingsFileName)) ? File.ReadAllText(SettingsFileName) : "";
        //    if (optionsJson != "" && optionsJson[0] == '{' && optionsJson[optionsJson.Length - 1] == '}')
        //    {
        //        instance = JsonUtility.FromJson<GameBalanceSettings>(optionsJson);
        //    }
        //    else
        //    {
        //        Create();
        //    }

        //    //Save to file to update any added or removed fields.
        //    string newOptionsJson = JsonUtility.ToJson(instance, true);
        //    File.WriteAllText(SettingsFileName, newOptionsJson);
        //}

        private static void Create()
        {
            instance = new GameBalanceSettings();
        }
    }
}
