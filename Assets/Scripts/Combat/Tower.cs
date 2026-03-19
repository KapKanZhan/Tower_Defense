using UnityEngine;

public class Tower : MonoBehaviour
{
    [System.Serializable]
    public class TowerLevelData
    {
        public int upgradeCost = 25;
        public float attackRange = 2.5f;
        public float attackCooldown = 1f;
        public int damage = 1;
    }

    [SerializeField] private TowerLevelData[] levels;
    [SerializeField] private Transform rangeCenter;

    private int currentLevelIndex = 0;
    private float cooldownTimer;

    public bool CanUpgrade => currentLevelIndex < levels.Length - 1;
    public int CurrentUpgradeCost => CanUpgrade ? levels[currentLevelIndex + 1].upgradeCost : -1;
    public int CurrentLevel => currentLevelIndex + 1;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        EnemyHealth target = FindTarget();
        if (target == null)
            return;

        if (cooldownTimer <= 0f)
        {
            Attack(target);
            cooldownTimer = GetCurrentData().attackCooldown;
        }
    }

    public bool TryUpgrade(PlayerMoney playerMoney)
    {
        if (!CanUpgrade || playerMoney == null)
            return false;

        int cost = CurrentUpgradeCost;

        if (!playerMoney.TrySpend(cost))
            return false;

        currentLevelIndex++;
        return true;
    }

    private TowerLevelData GetCurrentData()
    {
        if (levels == null || levels.Length == 0)
        {
            return new TowerLevelData();
        }

        return levels[currentLevelIndex];
    }

    private EnemyHealth FindTarget()
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        EnemyHealth closestEnemy = null;
        float closestDistance = float.MaxValue;

        Vector3 center = rangeCenter != null ? rangeCenter.position : transform.position;
        float range = GetCurrentData().attackRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyHealth enemy = enemies[i];

            if (enemy == null || enemy.IsDead)
                continue;

            float distance = Vector2.Distance(center, enemy.transform.position);

            if (distance <= range && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void Attack(EnemyHealth target)
    {
        if (target == null)
            return;

        target.TakeDamage(GetCurrentData().damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = rangeCenter != null ? rangeCenter.position : transform.position;
        float range = 4f;

        if (levels != null && levels.Length > 0)
            range = levels[Mathf.Clamp(currentLevelIndex, 0, levels.Length - 1)].attackRange;

        Gizmos.DrawWireSphere(center, range);
    }
}