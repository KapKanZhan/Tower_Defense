using UnityEngine;

public class TowerConstruction : MonoBehaviour
{
    private BuildSpot buildSpot;
    private GameObject towerPrefab;
    private Vector3 spawnPosition;
    private bool isCompleted;

    public void Init(BuildSpot targetBuildSpot, GameObject targetTowerPrefab, Vector3 targetPosition)
    {
        buildSpot = targetBuildSpot;
        towerPrefab = targetTowerPrefab;
        spawnPosition = targetPosition;
    }

    public void CompleteConstruction()
    {
        if (isCompleted)
            return;

        isCompleted = true;

        if (buildSpot == null || towerPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        GameObject tower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);
        buildSpot.FinishConstruction(tower);

        Destroy(gameObject);
    }
}