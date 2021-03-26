using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessWaveTD
{
    public static class EventConstants
    {
        public const string WaveChanged = "WaveChanged";
        public const string LiveChanged = "LivesChanged";
        public const string EnemyCountChanged = "EnemyCountChanged";
        public const string Reset = "Reset";
        public const string UpdateUi = "UpdateUi";

        public const string CancelClick = "CancelClick";
        public const string ClickOnPlayer = "ClickOnPlayer";

        //General Upgrades
        public const string UpdateGeneralUpgradesUi = "UpdateGeneralUpgradesUi";
        public const string UpdateTowersDamage = "UpdateTowersDamage";
        public const string UpdateTowersRange = "UpdateTowersRange";
        public const string UpdateTowersCritChance = "UpdateTowersCritChance";
        public const string UpdateTowersCritDamage = "UpdateTowersCritDamage";
        public const string UpdateStartingLives = "UpdateStartingLives";
        public const string UpdateStartingWave = "UpdateStartingWave";
        public const string UpdateExpDrop = "UpdateExpDrop";
    }
}