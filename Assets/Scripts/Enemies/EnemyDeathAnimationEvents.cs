using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyDeathAnimationEvents : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void OnDeathAnimationFinished()
    {
        if (enemyMovement != null)
            enemyMovement.RemoveEnemy();
    }
}