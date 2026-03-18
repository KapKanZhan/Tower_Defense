using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private WaveData[] waves;

    private int currentWaveIndex = -1;
    private bool isWaveRunning;
    private int aliveEnemies;
    private int activeLaneCoroutines;

    public int CurrentWaveNumber => currentWaveIndex + 1;
    public int TotalWaves => waves.Length;
    public bool IsWaveRunning => isWaveRunning;
    public bool HasMoreWaves => currentWaveIndex + 1 < waves.Length;

    public event Action<int, int> OnWaveStarted;
    public event Action<int, int> OnWaveCompleted;
    public event Action<int, int> OnWaveStateChanged;

    public void StartNextWave()
    {
        if (isWaveRunning || !HasMoreWaves)
            return;

        currentWaveIndex++;
        StartCoroutine(SpawnWaveRoutine(waves[currentWaveIndex]));
    }

    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        isWaveRunning = true;
        aliveEnemies = 0;
        activeLaneCoroutines = 0;

        OnWaveStarted?.Invoke(CurrentWaveNumber, TotalWaves);
        OnWaveStateChanged?.Invoke(CurrentWaveNumber, TotalWaves);

        if (wave.lanes != null)
        {
            for (int i = 0; i < wave.lanes.Length; i++)
            {
                WaveLane lane = wave.lanes[i];

                if (lane.path == null || lane.enemyCount <= 0)
                    continue;

                activeLaneCoroutines++;
                StartCoroutine(SpawnLaneRoutine(lane));
            }
        }

        while (activeLaneCoroutines > 0 || aliveEnemies > 0)
        {
            yield return null;
        }

        isWaveRunning = false;

        OnWaveCompleted?.Invoke(CurrentWaveNumber, TotalWaves);
        OnWaveStateChanged?.Invoke(CurrentWaveNumber, TotalWaves);
    }

    private IEnumerator SpawnLaneRoutine(WaveLane lane)
    {
        for (int i = 0; i < lane.enemyCount; i++)
        {
            SpawnEnemy(lane.path);
            yield return new WaitForSeconds(lane.spawnDelay);
        }

        activeLaneCoroutines--;
    }

    private void SpawnEnemy(EnemyPath enemyPath)
    {
        GameObject enemyObject = Instantiate(enemyPrefab);
        EnemyMovement enemyMovement = enemyObject.GetComponent<EnemyMovement>();

        if (enemyMovement == null || enemyPath == null || baseHealth == null)
            return;

        aliveEnemies++;
        enemyMovement.Init(enemyPath.Waypoints, baseHealth);
        enemyMovement.OnEnemyRemoved += HandleEnemyRemoved;
    }

    private void HandleEnemyRemoved(EnemyMovement enemy)
    {
        enemy.OnEnemyRemoved -= HandleEnemyRemoved;
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }
}