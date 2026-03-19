using System;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damageToBase = 1;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private BaseHealth baseHealth;
    private EnemyHealth enemyHealth;

    private bool isRemoved;
    private bool reachedEnd;
    private bool isDying;

    public bool HasReachedEnd => reachedEnd;

    public event Action<EnemyMovement> OnEnemyRemoved;

    public Vector2 CurrentDirection { get; private set; }

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
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
        if (isRemoved || reachedEnd || isDying)
            return;

        if (enemyHealth != null && enemyHealth.IsDead)
            return;

        if (waypoints == null || waypoints.Length == 0)
            return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            ReachEnd();
            return;
        }

        Transform targetPoint = waypoints[currentWaypointIndex];
        Vector3 toTarget = targetPoint.position - transform.position;
        CurrentDirection = toTarget.normalized;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
            currentWaypointIndex++;
    }

    private void ReachEnd()
    {
        if (reachedEnd)
            return;

        reachedEnd = true;

        if (baseHealth != null)
            baseHealth.TakeDamage(damageToBase);

        RemoveEnemy();
    }

    private void HandleDeath(EnemyHealth health)
    {
        if (isDying)
            return;

        isDying = true;
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