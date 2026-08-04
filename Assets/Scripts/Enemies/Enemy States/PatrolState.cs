// PatrolState.cs
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    [SerializeField] protected float patrolDistance = 5f;
    public override void FixedTick()
    {
        float distanceMoved = Vector3.Distance(startPosition, transform.position);
        bool movingAway = (transform.position.x > startPosition.x && enemy.CurrentDirection > 0) ||
                        (transform.position.x < startPosition.x && enemy.CurrentDirection < 0);

        if (distanceMoved >= patrolDistance && movingAway)
            enemy.SetDirection(-enemy.CurrentDirection);

        rb.linearVelocityX = enemy.MoveSpeed * enemy.CurrentDirection;
        sprite.flipX = enemy.CurrentDirection < 0;

        if (CheckForPlayerLOS())
        {
            animator.SetBool("Walking", false);
            enemy.GoTo<EnemyAttackState>();
        }
    }
}