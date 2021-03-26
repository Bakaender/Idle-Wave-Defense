using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace EndlessWaveTD
{
    public class SaveGame
    {
        static protected SaveGame instance;
        public static SaveGame Instance
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

        //TODO Implement.
        //Stats to track
        public int EnemyKills = 0;
        public int TotalExpGained = 0;
        public int WavesCleared = 0;

        //Player Stats
        public BigDouble PlayerExp;
        public BigDouble PlayerRankPoints;
        public long PlayerPremiumCurrency;

        //Active Towers
        public bool PhysicalTowerActive = true;
        public bool FireTowerActive;
        public bool IceTowerActive;
        public bool LightningTowerActive;
        public bool PoisonTowerActive;

        //General Upgrades.
        public int[] GeneralUpgradeLevels;

        protected string saveFile = "";

        static int s_Version = 1;

        #region Access Methods

        //public bool ActivateTower(float cost, TowerTypes type)
        //{
        //    switch (type)
        //    {
        //        case TowerTypes.Fire:
        //            if (!FireTowerActive && PlayerExp >= cost)
        //            {
        //                PlayerExp -= cost;
        //                FireTowerActive = true;
        //                SaveGame.instance.Save();
        //                return true;
        //            }
        //            break;

        //        case TowerTypes.Ice:
        //            if (!IceTowerActive && PlayerExp >= cost)
        //            {
        //                PlayerExp -= cost;
        //                IceTowerActive = true;
        //                SaveGame.instance.Save();
        //                return true;
        //            }
        //            break;

        //        case TowerTypes.Lightning:
        //            if (!LightningTowerActive && PlayerExp >= cost)
        //            {
        //                PlayerExp -= cost;
        //                LightningTowerActive = true;
        //                SaveGame.instance.Save();
        //                return true;
        //            }
        //            break;

        //        case TowerTypes.Poison:
        //            if (!PoisonTowerActive && PlayerExp >= cost)
        //            {
        //                PlayerExp -= cost;
        //                PoisonTowerActive = true;
        //                SaveGame.instance.Save();
        //                return true;
        //            }
        //            break;

        //        case TowerTypes.Physical:
        //        default:
        //            break;
        //    }

        //    return false;
        //}

        //public void BuyGeneralUpgrade(BigDouble cost, GeneralUpgradesEnum id, int numberToBuy)
        //{
        //    if (PlayerExp >= cost)
        //    {
        //        PlayerExp -= cost;
        //        GeneralUpgradeLevels[(int)id] += numberToBuy;

        //        if (MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].ConstantUpgrade)
        //        {
        //            MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].Value +=
        //                MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].ConstantUpgradeValue * numberToBuy;
        //        }
        //        else
        //        {
        //            MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].Value += MathFormulas.BulkUpgradeValue(
        //                MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].Value,
        //                MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].ScalePercent, 
        //                numberToBuy) / 100;
        //        }

        //        MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].UpgradeCost = MathFormulas.GetNextLevelUpgradeCost(
        //            new BigDouble(MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].UpgradeCostDigits, MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].UpgradeCostExponent),
        //            MainReferences.generalUpgradesManager.GeneralUpgradesArray[(int)id].UpgradeCostScalePercent, 
        //            GeneralUpgradeLevels[(int)id]);

        //        if (id == GeneralUpgradesEnum.OverallDamage)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateTowersDamage);
        //        }
        //        else if (id == GeneralUpgradesEnum.OverallRange)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateTowersRange);
        //        }
        //        else if (id == GeneralUpgradesEnum.OverallCritChance)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateTowersCritChance);
        //        }
        //        else if (id == GeneralUpgradesEnum.OverallCritDamage)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateTowersDamage);
        //        }
        //        else if (id == GeneralUpgradesEnum.StartingLives)
        //        {
        //            MainReferences.generalUpgradesManager.currentLives += numberToBuy;
        //            EventManager.TriggerEvent(EventConstants.LiveChanged);
        //        }
        //        else if (id == GeneralUpgradesEnum.StartingWave)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateStartingWave);
        //        }
        //        else if (id == GeneralUpgradesEnum.ExpDrop)
        //        {
        //            EventManager.TriggerEvent(EventConstants.UpdateExpDrop);
        //        }

        //        EventManager.TriggerEvent(EventConstants.UpdateGeneralUpgradesUi);

        //        SaveGame.instance.Save();
        //    }
        //    else
        //    {
        //        //TODO not enough exp to buy message.
        //        Debug.Log("Not enough exp to buy " + id + " Need " + cost.ToString());
        //    }
        //}

        #endregion

        #region Create/Load/Save

        static public void Create()
        {
            if (instance == null)
            {
                instance = new SaveGame();
            }

            instance.saveFile = Application.persistentDataPath + "/DevSave2.bin";

            if (File.Exists(instance.saveFile))
            {
                // If we have a save, we load it.
                instance.Load();
            }
            else
            {
                // If not we create one with default data.
                NewSaveGame();
            }
        }

        static public void NewSaveGame()
        {
            instance.EnemyKills = 0;
            instance.TotalExpGained = 0;
            instance.WavesCleared = 0;
            instance.PlayerExp = new BigDouble(AllSettings.Instance.StartingExp);
            instance.PlayerRankPoints = new BigDouble(AllSettings.Instance.StartingRankPoints);
            instance.PlayerPremiumCurrency = AllSettings.Instance.StartingPremiumCurrency;

            //TODO Set to this after testing everything.
            //instance.PhysicalTowerActive = true;
            //instance.FireTowerActive = false;
            //instance.IceTowerActive = false;
            //instance.LightningTowerActive = false;
            //instance.PoisonTowerActive = false;

            instance.PhysicalTowerActive = true;
            instance.FireTowerActive = true;
            instance.IceTowerActive = true;
            instance.LightningTowerActive = true;
            instance.PoisonTowerActive = true;

            instance.GeneralUpgradeLevels = new int[System.Enum.GetNames(typeof(GeneralUpgradesEnum)).Length];

            instance.Save();
        }

        public void Load()
        {
            BinaryReader r = new BinaryReader(new FileStream(saveFile, FileMode.Open));

#pragma warning disable
            int version = r.ReadInt32();
#pragma warning restore

            EnemyKills = r.ReadInt32();
            TotalExpGained = r.ReadInt32();
            WavesCleared = r.ReadInt32();
            PlayerExp = new BigDouble(r.ReadDouble(), r.ReadInt64());
            PlayerRankPoints = new BigDouble(r.ReadDouble(), r.ReadInt64());
            PlayerPremiumCurrency = r.ReadInt64();

            if (r.ReadInt32() == 1) PhysicalTowerActive = true;
            else PhysicalTowerActive = false;

            if (r.ReadInt32() == 1) FireTowerActive = true;
            else FireTowerActive = false;

            if (r.ReadInt32() == 1) IceTowerActive = true;
            else IceTowerActive = false;

            if (r.ReadInt32() == 1) LightningTowerActive = true;
            else LightningTowerActive = false;

            if (r.ReadInt32() == 1) PoisonTowerActive = true;
            else PoisonTowerActive = false;

            //Don't remember how to handle increasing enum size etc.
            //Load them into a temp array, compare sizes, copy data to real one.
            //TODO Right array size. Something like if version... resize array after reading to new right size for next save.
            GeneralUpgradeLevels = new int[r.ReadInt32()];
            for (int i = 0; i < GeneralUpgradeLevels.Length; i++)
            {
                GeneralUpgradeLevels[i] = r.ReadInt32();
            }

            r.Close();
        }

        public void Save()
        {
            BinaryWriter w = new BinaryWriter(new FileStream(saveFile, FileMode.OpenOrCreate));

            w.Write(s_Version);

            w.Write(instance.EnemyKills);
            w.Write(instance.TotalExpGained);
            w.Write(instance.WavesCleared);

            w.Write(instance.PlayerExp.Number);
            w.Write(instance.PlayerExp.Exponent);

            w.Write(instance.PlayerRankPoints.Number);
            w.Write(instance.PlayerRankPoints.Exponent);

            w.Write(instance.PlayerPremiumCurrency);

            if (PhysicalTowerActive) w.Write(1);
            else w.Write(0);

            if (FireTowerActive) w.Write(1);
            else w.Write(0);

            if (IceTowerActive) w.Write(1);
            else w.Write(0);

            if (LightningTowerActive) w.Write(1);
            else w.Write(0);

            if (PoisonTowerActive) w.Write(1);
            else w.Write(0);

            w.Write(instance.GeneralUpgradeLevels.Length);
            for (int i = 0; i < instance.GeneralUpgradeLevels.Length; i++)
            {
                w.Write(instance.GeneralUpgradeLevels[i]);
            }

            w.Close();
        }

        #endregion
    }
}

