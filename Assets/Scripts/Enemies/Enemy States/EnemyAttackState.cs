using UnityEngine;

public class EnemyAttackState : EnemyState
{
    [SerializeField] private float backupDetectionDistance;
    [SerializeField] private float backupDuration = 0.5f;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private float fireCooldown = 1.5f;
    private float fireTimer;
    private float backupTimer;

    public override void Enter()
    {
        backupTimer = 0f;
        fireTimer = 0f; // fires immediately on entering attack; set to fireCooldown instead to force a wait first
    }

    public override void FixedTick()
    {
        if (backupTimer > 0)
            backupTimer -= Time.fixedDeltaTime;

        if (fireTimer > 0)
            fireTimer -= Time.fixedDeltaTime;

        if (IsPlayerClose())
            backupTimer = backupDuration;

        if (backupTimer > 0)
        {
            animator.ResetTrigger("Attacking");
            animator.SetBool("Backup", true);
            rb.linearVelocityX = enemy.MoveSpeed * enemy.CurrentDirection * -.5f;
        }
        else
        {
            animator.SetBool("Backup", false);

            if (fireTimer <= 0)
            {
                animator.SetTrigger("Attacking");
                Instantiate(bullet, FirePoint.position, FirePoint.rotation);
                fireTimer = fireCooldown;
            }
        }

        sprite.flipX = enemy.CurrentDirection < 0;

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
        Vector2 facing = enemy.CurrentDirection < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, facing, backupDetectionDistance, playerMask);

        Debug.DrawRay(rayOrigin, (Vector3)(facing * backupDetectionDistance), Color.red);
        return hit.collider != null;
    }
}