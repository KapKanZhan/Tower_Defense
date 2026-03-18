using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        if (enemySpawner == null)
            return;

        enemySpawner.OnWaveStarted += HandleWaveStarted;
        enemySpawner.OnWaveCompleted += HandleWaveCompleted;
        UpdateIdleState();
    }

    private void OnDestroy()
    {
        if (enemySpawner == null)
            return;

        enemySpawner.OnWaveStarted -= HandleWaveStarted;
        enemySpawner.OnWaveCompleted -= HandleWaveCompleted;
    }

    private void HandleWaveStarted(int current, int total)
    {
        waveText.text = $"{current}/{total}";
        statusText.text = "Наступает волна";
    }

    private void HandleWaveCompleted(int current, int total)
    {
        waveText.text = $"{current}/{total}";
        statusText.text = enemySpawner.HasMoreWaves ? "Конец волны" : "Всем волнам конец";
    }

    private void UpdateIdleState()
    {
        if (enemySpawner.TotalWaves > 0)
        {
            waveText.text = $"0/{enemySpawner.TotalWaves}";
            statusText.text = "Начните волну";
        }
        else
        {
            waveText.text = "0/0";
            statusText.text = "Нет волн";
        }
    }
}