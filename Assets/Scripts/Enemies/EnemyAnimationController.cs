using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private EnemyMovement movement;
    private EnemyHealth health;

    private string currentAnimation;
    private DirectionType lastDirection = DirectionType.Down;

    private enum DirectionType
    {
        Down,
        Side,
        Up
    }

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        health = GetComponent<EnemyHealth>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animator == null || movement == null || health == null)
            return;

        if (health.IsDead)
        {
            PlayDeathAnimation();
            return;
        }

        UpdateMoveAnimation();
    }

    private void UpdateMoveAnimation()
    {
        Vector2 dir = movement.CurrentDirection;

        if (dir.sqrMagnitude <= 0.001f)
            return;

        DirectionType direction;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            direction = DirectionType.Side;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = dir.x > 0f;
            }
        }
        else
        {
            direction = dir.y > 0f ? DirectionType.Up : DirectionType.Down;
        }

        lastDirection = direction;

        switch (direction)
        {
            case DirectionType.Up:
                PlayAnimation("Walk_Up");
                break;
            case DirectionType.Down:
                PlayAnimation("Walk_Down");
                break;
            case DirectionType.Side:
                PlayAnimation("Walk_Side");
                break;
        }
    }

    private void PlayDeathAnimation()
    {
        switch (lastDirection)
        {
            case DirectionType.Up:
                PlayAnimation("Death_Up");
                break;
            case DirectionType.Down:
                PlayAnimation("Death_Down");
                break;
            case DirectionType.Side:
                PlayAnimation("Death_Side");
                break;
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
            return;

        currentAnimation = animationName;
        animator.Play(animationName);
    }
}