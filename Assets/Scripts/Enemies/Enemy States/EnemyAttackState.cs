using UnityEngine;

public class EnemyAttackState : EnemyState
{
    [SerializeField] private float backupDetectionDistance;
    [SerializeField] private float backupDuration = 0.5f;
    
    public override void Enter()
    {
        backupTimer = 0f;
    }

    public override void FixedUpdate()
    {
        if (backupTimer > 0)
            backupTimer -= Time.fixedDeltaTime;

        if (IsPlayerClose())
            backupTimer = backupDuration;

        if (backupTimer > 0)
        {
            animator.ResetTrigger("Attacking");
            animator.SetBool("Backup", true);
            rb.linearVelocityX = moveSpeed * -.5f;
        }
        else
        {
            animator.SetBool("Backup", false);
            animator.SetTrigger("Attacking");
        }

        sprite.flipX = moveSpeed < 0;

        if (!CheckForPlayerLOS())
        {
            animator.ResetTrigger("Attacking");
            animator.SetTrigger("Walking");
            enemy.GoTo<EnemyPatrolState>();
        }
    }

    protected bool IsPlayerClose()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, .25f, 0);
        Vector2 facing = enemy.MoveSpeed < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, facing, backupDetectionDistance, playerMask);
        
        Debug.DrawRay(rayOrigin, (Vector3)(facing * backupDetectionDistance), Color.red);
        return hit.collider != null;
    }
}