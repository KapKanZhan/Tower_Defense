using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    public GameObject enemyPrefab;
    public int enemyCount = 1;
    public float spawnDelay = 1f;
}