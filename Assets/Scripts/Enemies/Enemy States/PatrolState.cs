// PatrolState.cs
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    [SerializeField] protected float patrolDistance = 5f;
    public override void FixedUpdate()
    {
        float distanceMoved = Vector3.Distance(startPosition, transform.position);
        bool movingAway = (transform.position.x > startPosition.x && moveSpeed > 0) ||
                           (transform.position.x < startPosition.x && moveSpeed < 0);

        if (distanceMoved >= patrolDistance && movingAway)
            moveSpeed *= -1;

        rb.linearVelocityX = moveSpeed;
        sprite.flipX = moveSpeed < 0;

        if (CheckForPlayerLOS())
        {
            animator.SetBool("Walking", false);
            enemy.GoTo<EnemyAttackState>();
        }
    }
}