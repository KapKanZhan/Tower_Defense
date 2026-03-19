using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyReward : MonoBehaviour
{
    [SerializeField] private int reward = 5;

    private EnemyHealth enemyHealth;
    private EnemyMovement enemyMovement;
    private bool rewardGiven;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
            enemyHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
            enemyHealth.OnDeath -= HandleDeath;
    }

    private void HandleDeath(EnemyHealth health)
    {
        if (rewardGiven)
            return;

        if (enemyMovement != null && enemyMovement.HasReachedEnd)
            return;

        PlayerMoney playerMoney = FindFirstObjectByType<PlayerMoney>();

        if (playerMoney != null)
        {
            playerMoney.AddMoney(reward);
            rewardGiven = true;
        }
    }
}