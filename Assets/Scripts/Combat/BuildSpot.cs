using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    [SerializeField] private GameObject constructionPrefab;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Transform towerSpawnPoint;
    [SerializeField] private SpriteRenderer platformRenderer;
    [SerializeField] private int buildCost = 50;

    private bool isOccupied;
    private bool isConstructing;
    private GameObject currentTower;

    private PlayerMoney playerMoney;

    private void Start()
    {
        playerMoney = FindFirstObjectByType<PlayerMoney>();
    }

    private void OnMouseDown()
    {
        if (isConstructing)
            return;

        if (!isOccupied)
        {
            StartConstruction();
            return;
        }

        TryUpgradeTower();
    }

    private void StartConstruction()
    {
        if (playerMoney == null || !playerMoney.TrySpend(buildCost))
            return;

        isConstructing = true;

        if (platformRenderer != null)
            platformRenderer.enabled = false;

        Vector3 spawnPosition = towerSpawnPoint != null ? towerSpawnPoint.position : transform.position;

        GameObject constructionObject = Instantiate(constructionPrefab, spawnPosition, Quaternion.identity);
        TowerConstruction construction = constructionObject.GetComponent<TowerConstruction>();

        if (construction != null)
        {
            construction.Init(this, towerPrefab, spawnPosition);
        }
        else
        {
            CancelConstruction();
        }
    }

    private void TryUpgradeTower()
    {
        if (currentTower == null || playerMoney == null)
            return;

        Tower tower = currentTower.GetComponent<Tower>();

        if (tower == null)
            return;

        tower.TryUpgrade(playerMoney);
    }

    public void FinishConstruction(GameObject builtTower)
    {
        isConstructing = false;
        isOccupied = true;
        currentTower = builtTower;
    }

    public void CancelConstruction()
    {
        isConstructing = false;
        isOccupied = false;

        if (platformRenderer != null)
            platformRenderer.enabled = true;
    }
}