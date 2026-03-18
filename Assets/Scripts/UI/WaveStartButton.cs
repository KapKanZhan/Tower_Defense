using UnityEngine;

public class WaveStartButton : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    public void StartNextWave()
    {
        if (enemySpawner != null)
        {
            enemySpawner.StartNextWave();
        }
    }
}