using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessWaveTD
{
    public enum GameState
    {
        Playing,
        Died,
        Reseting
    }

    public class GameDataManager : MonoBehaviour
    {
        public static GameState gameState = GameState.Playing;

        public const int EnemyHpDigitsOfPrecision = 10;
        public static World DefaultWorld => World.DefaultGameObjectInjectionWorld;

        public static GameDataManager instance;
        public EntityManager Manager;
        public BlobAssetStore BlobStore;
        public GameObjectConversionSettings Settings;

        private void Awake()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif

            //QualitySettings.vSyncCount = 0;

            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
                instance = this;

            Manager = DefaultWorld.EntityManager;
            BlobStore = new BlobAssetStore();
            Settings = GameObjectConversionSettings.FromWorld(DefaultWorld, BlobStore);
        }

        private void OnDestroy()
        {
            BlobStore.Dispose();
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.V))
            //{
            //    QualitySettings.vSyncCount = 0; //unlimited fps
            //}
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    QualitySettings.vSyncCount = 1; //60 fps
            //}
            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    QualitySettings.vSyncCount = 2; //30 fps
            //}
            //if (Input.GetKeyDown(KeyCode.M))
            //{
            //    QualitySettings.vSyncCount = 3; //20fps
            //}

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4f;
            }
        }
    }
}
