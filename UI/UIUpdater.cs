using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;

namespace EndlessWaveTD
{
    public class UIUpdater : MonoBehaviour
    {
        //public GameObject UpgradeUiTemplatePrefab;

        public TMP_Text UiWaveText;
        public TMP_Text EnemiesRemainText;
        public TMP_Text PlayerLivesText;

        [Header("Upgrade UI")]
        public TMP_Text ExpText;
        public TMP_Text RankText;
        public TMP_Text PremiumCurrencyText;
        public float UpdateUiTextInterval = 0.1f;

        private void OnEnable()
        {
            EventManager.StartListening(EventConstants.UpdateUi, UpdateUI);

            StartCoroutine(UpdateExp());
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventConstants.UpdateUi, UpdateUI);
        }

        private void UpdateUI()
        {
            UiWaveUpdate();
            LivesUiUpdate();
            EnemiesRemainUpdate();
        }

        private void UiWaveUpdate()
        {
            UiWaveText.text = "Wave " + (MainReferences.waveSpawner.CurrentWave + 1);
            EnemiesRemainText.text = MainReferences.waveSpawner.nextWaveEnemies + " Enemies Left";
        }

        private void LivesUiUpdate()
        {
            PlayerLivesText.text = "Lives: " + MainReferences.waveSpawner.CurrentLives;
        }

        private void EnemiesRemainUpdate()
        {
            EnemiesRemainText.text = MainReferences.waveSpawner.nextWaveEnemies - MainReferences.waveSpawner.enemiesKilledThisWave + " Enemies Left";
        }

        IEnumerator UpdateExp()
        {
            for (; ; )
            {
                ExpText.text = "Current Exp:\n" + SaveGame.Instance.PlayerExp.ToString();
                RankText.text = "Current Rank Points:\n" + SaveGame.Instance.PlayerRankPoints.ToString();
                PremiumCurrencyText.text = "Current Gems or ?:\n" + SaveGame.Instance.PlayerPremiumCurrency.ToString();

                yield return new WaitForSeconds(UpdateUiTextInterval);
            }
        }
    }
}