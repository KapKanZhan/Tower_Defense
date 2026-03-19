using System.Collections;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private float victoryDelay = 1.5f;

    private bool victoryShown;

    private void Update()
    {
        if (victoryShown || enemySpawner == null)
            return;

        if (!enemySpawner.HasMoreWaves &&
            !enemySpawner.IsWaveRunning &&
            enemySpawner.AliveEnemies <= 0)
        {
            StartCoroutine(ShowVictoryWithDelay());
            victoryShown = true;
        }
    }

    private IEnumerator ShowVictoryWithDelay()
    {
        yield return new WaitForSeconds(victoryDelay);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}