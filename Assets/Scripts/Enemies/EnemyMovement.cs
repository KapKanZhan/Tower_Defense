using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damageToBase = 1;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private BaseHealth baseHealth;
    private bool isRemoved;

    public event Action<EnemyMovement> OnEnemyRemoved;

    public void Init(Transform[] pathPoints, BaseHealth targetBase)
    {
        waypoints = pathPoints;
        baseHealth = targetBase;

        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            currentWaypointIndex = 1;
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            ReachEnd();
            return;
        }

        Transform targetPoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            currentWaypointIndex++;
        }
    }

    private void ReachEnd()
    {
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(damageToBase);
        }

        RemoveEnemy();
    }

    public void RemoveEnemy()
    {
        if (isRemoved)
            return;

        isRemoved = true;
        OnEnemyRemoved?.Invoke(this);
        Destroy(gameObject);
    }
}