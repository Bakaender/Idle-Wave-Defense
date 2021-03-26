using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using EndlessWaveTD;
using System;
using Unity.Rendering;
using Unity.Collections;

public class WaveSpawnerECS : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject EnemySpritePrefab;
    private Queue<EnemyFollow> EnemySpriteQueue;
    public float EnemyMoveSpeed = 0.25f;

    [Header("Base Enemy Health")]
    [SerializeField] private double baseHealthDigits;
    [SerializeField] private long baseHealthExponent;
    [SerializeField] private float baseHealthScaleFactor;

    [Header("Wave Settings")]
    [SerializeField] private float WaveTotalEnemySpawnTime = 30f;
    public int FirstWaveEnemyCount = 25;
    [SerializeField] private float EnemyCountScalePercent = 1.1f;

    private float resetDelay;

    private int MaxEnemyCount;
    
    public int CurrentWave { get; private set; } = 0;

    [HideInInspector] public int CurrentLives;

    [HideInInspector] public int enemiesKilledThisWave = 0;

    private double enemyHpResetDigits;
    private long enemyHpResetExponent;

    //To not overwrite basehp for when reseting wave, this will be the one to set hp from and calculate percents.
    public static BigDouble currentWaveEnemyHp;

    [HideInInspector] public int nextWaveEnemies;
    [HideInInspector] public int enemiesSpawned = 0;
    private float nextSpawnTime = 0f;

    private Entity enemyPrefabEntity;

    private void Awake()
    {
        MainReferences.waveSpawner = this;
        MaxEnemyCount = AllSettings.Instance.MaxEnemiesPerWave;
        CurrentLives = AllSettings.Instance.StartingLives;
    }

    void Start()
    {
        enemyPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(EnemyPrefab, GameDataManager.instance.Settings);

        if (FirstWaveEnemyCount > MaxEnemyCount)
        {
            FirstWaveEnemyCount = MaxEnemyCount;
        }
        currentWaveEnemyHp = new BigDouble(baseHealthDigits, baseHealthExponent);
        //To avoid creating a new BigDouble while still getting num and exp I want. Using base wouldn't go through the constructor.
        enemyHpResetDigits = currentWaveEnemyHp.Number;
        enemyHpResetExponent = currentWaveEnemyHp.Exponent;

        ResetWave();

        EnemySpriteQueue = new Queue<EnemyFollow>();
    }

    private void ResetWave()
    {
        CurrentLives = AllSettings.Instance.StartingLives;
        CurrentLives += SaveGame.Instance.GeneralUpgradeLevels[(int)GeneralUpgradesEnum.StartingLives];

        switch (GameDataManager.gameState)
        {
            case GameState.Playing:
            case GameState.Reseting:
            default:
                CurrentWave = 0;
                CurrentWave += SaveGame.Instance.GeneralUpgradeLevels[(int)GeneralUpgradesEnum.StartingWave];
                break;

            case GameState.Died:
                if (CurrentWave > 0)
                {
                    CurrentWave--;
                }
                break;   
        }

        //Reset everything back to base.
        currentWaveEnemyHp.Number = enemyHpResetDigits;
        currentWaveEnemyHp.Exponent = enemyHpResetExponent;

        nextWaveEnemies = FirstWaveEnemyCount;
        enemiesSpawned = 0;
        enemiesKilledThisWave = 0;
        nextSpawnTime = 0;

        //Recalculate values for current wave.
        for (int i = 0; i < CurrentWave; i++)
        {
            nextWaveEnemies = (int)(nextWaveEnemies * EnemyCountScalePercent);
            if (nextWaveEnemies > MaxEnemyCount)
            {
                nextWaveEnemies = MaxEnemyCount;
            }
            currentWaveEnemyHp *= baseHealthScaleFactor;
        }

        EventManager.TriggerEvent(EventConstants.WaveChanged);
        EventManager.TriggerEvent(EventConstants.UpdateUi);
    }

    //Reset on GameOver
    private void ResetGameOver()
    {
        CurrentLives = AllSettings.Instance.StartingLives;
        CurrentLives += SaveGame.Instance.GeneralUpgradeLevels[(int)GeneralUpgradesEnum.StartingLives];

        if (CurrentWave > 0)
        {
            CurrentWave -= 1;
        }  

        //Reset everything back to base.
        currentWaveEnemyHp.Number = enemyHpResetDigits;
        currentWaveEnemyHp.Exponent = enemyHpResetExponent;

        nextWaveEnemies = FirstWaveEnemyCount;
        enemiesSpawned = 0;
        enemiesKilledThisWave = 0;
        nextSpawnTime = 0;

        //Recalculate values for current wave.
        for (int i = 0; i < CurrentWave; i++)
        {
            nextWaveEnemies = (int)(nextWaveEnemies * EnemyCountScalePercent);
            if (nextWaveEnemies > MaxEnemyCount)
            {
                nextWaveEnemies = MaxEnemyCount;
            }
            currentWaveEnemyHp *= baseHealthScaleFactor;
        }

        EventManager.TriggerEvent(EventConstants.WaveChanged);
        EventManager.TriggerEvent(EventConstants.UpdateUi);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventConstants.LiveChanged, LifeLostUpdater);
        EventManager.StartListening(EventConstants.EnemyCountChanged, EnemyKilledUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventConstants.LiveChanged, LifeLostUpdater);
        EventManager.StopListening(EventConstants.EnemyCountChanged, EnemyKilledUpdate);
    }

    private void Update()
    {
        switch (GameDataManager.gameState)
        {
            case GameState.Playing:
                nextSpawnTime -= Time.deltaTime;

                if (enemiesSpawned < nextWaveEnemies && nextSpawnTime <= 0)
                {
                    SpawnEnemy();
                    enemiesSpawned++;
                    nextSpawnTime = WaveTotalEnemySpawnTime / nextWaveEnemies;
                }

                if (enemiesKilledThisWave >= nextWaveEnemies)
                {
                    NextWave();
                }
                break;

            case GameState.Died:
                resetDelay -= Time.unscaledDeltaTime;
                if (resetDelay <= 0)
                {
                    GameReset();
                }
                break;

            case GameState.Reseting:
                resetDelay -= Time.unscaledDeltaTime;
                if (resetDelay <= 0)
                {
                    GameReset();
                }
                break;

            default:
                break;
        }
    }

    private void SpawnEnemy()
    {
        Entity enemy = GameDataManager.instance.Manager.Instantiate(enemyPrefabEntity);
        Vector3 position = MathFormulas.RandomCircle(Vector3.zero, AllSettings.Instance.GameAreaEdgeDistance, UnityEngine.Random.Range(-180f, 180f));
        GameDataManager.instance.Manager.SetComponentData(enemy, new Translation { Value = position });
        GameDataManager.instance.Manager.SetComponentData(enemy, new EnemyData
        {
            HealthNum = currentWaveEnemyHp.Number,
            HealthExpo = currentWaveEnemyHp.Exponent,
            moveSpeed = EnemyMoveSpeed,
            moveDistance = position.normalized,
            HpPercent = 1,
            Position = position
        });

        if (EnemySpriteQueue.Count == 0 || EnemySpriteQueue.Peek().following)
        {
            EnemyFollow enemySprite = Instantiate(EnemySpritePrefab).GetComponent<EnemyFollow>();
            enemySprite.NewEnemy(enemy);
            EnemySpriteQueue.Enqueue(enemySprite);
        }
        else
        {
            EnemyFollow enemySprite = EnemySpriteQueue.Dequeue();
            enemySprite.NewEnemy(enemy);
            EnemySpriteQueue.Enqueue(enemySprite);
        }
    }

    private void NextWave()
    {
        nextWaveEnemies = (int)(nextWaveEnemies * EnemyCountScalePercent);
        if (nextWaveEnemies > MaxEnemyCount)
        {
            nextWaveEnemies = MaxEnemyCount;
        }

        currentWaveEnemyHp *= baseHealthScaleFactor;
        enemiesSpawned = 0;
        enemiesKilledThisWave = 0;
        nextSpawnTime = 0;

        CurrentWave++;
        EventManager.TriggerEvent(EventConstants.WaveChanged);
        EventManager.TriggerEvent(EventConstants.UpdateUi);
    }

    public void Revive()
    {
        if (CurrentWave >= AllSettings.Instance.MinimumReviveWave - 1)
        {
            resetDelay = AllSettings.Instance.ReviveResetTime;
            GameDataManager.gameState = GameState.Reseting;
            EventManager.TriggerEvent(EventConstants.Reset);
            EventManager.TriggerEvent(EventConstants.UpdateUi);

            //TODO give revive bonuses etc.
        }
    }

    private void LifeLostUpdater()
    {
        enemiesKilledThisWave++;
        CurrentLives--;
        if (CurrentLives < 1)
        {
            CurrentLives = 0;
            resetDelay = AllSettings.Instance.GameOverResetTime;
            GameDataManager.gameState = GameState.Died;

            EventManager.TriggerEvent(EventConstants.Reset);           
        }

        EventManager.TriggerEvent(EventConstants.UpdateUi);
    }

    //For Revives
    private void GameReset()
    {
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Prefab), typeof(PlayerData) },
            All = new ComponentType[] { typeof(Translation) }
        };
        //EntityQuery m_Group = GameDataManager.instance.Manager.CreateEntityQuery(query);

        //GameDataManager.instance.Manager.DestroyEntity(GameDataManager.instance.Manager.CreateEntityQuery(query));
        EntityQuery toDestroy = GameDataManager.instance.Manager.CreateEntityQuery(query);
        GameDataManager.instance.Manager.DestroyEntity(toDestroy);
        toDestroy.Dispose();

        ResetWave();

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        GameDataManager.gameState = GameState.Playing;
    }

    private void EnemyKilledUpdate()
    {
        enemiesKilledThisWave++;
        SaveGame.Instance.EnemyKills++;
        //TODO Give Right Exp
        SaveGame.Instance.PlayerExp += 1;

        EventManager.TriggerEvent(EventConstants.UpdateUi);
    }
}
