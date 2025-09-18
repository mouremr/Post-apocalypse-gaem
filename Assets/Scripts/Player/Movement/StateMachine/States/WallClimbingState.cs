using UnityEngine;

public class WallClimbingState : PlayerState
{

    float yVelocity;

    float facingDirection;

    private float wallExitCooldown = 0.2f;
    private float wallExitTimer = 0f;
    public WallClimbingState(StateMachine stateMachine) : base(stateMachine)
    {
        facingDirection = spriteRenderer.flipX ? -1f : 1f;
        wallExitTimer = wallExitCooldown; // start timer

    }

    public override void Enter()
    {
        animator.SetBool("jumping", false);

    }

    public override void Update()
    {
        float slideSpeed = -2f; // units per second
        rb.linearVelocity = new Vector2(0f, slideSpeed);

        if (wallExitTimer > 0f)
            wallExitTimer -= Time.deltaTime;

        if (wallExitTimer <= 0f && Mathf.Sign(input.HorizontalInput) != facingDirection && input.HorizontalInput != 0)
        {
            animator.SetBool("grounded", true);
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }

    }

    private bool IsWalled()
    {
        //get position of wallcheck obejct
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.05f, 0.5f) , 0, climbable);
    }

    private bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        Vector2 originLeft = new Vector2(bounds.min.x + 0.1f, bounds.min.y);
        Vector2 originCenter = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 originRight = new Vector2(bounds.max.x - 0.1f, bounds.min.y);

        float rayDistance = 0.1f;
        LayerMask groundMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.down, rayDistance, groundMask);
        RaycastHit2D hitCenter = Physics2D.Raycast(originCenter, Vector2.down, rayDistance, groundMask);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.down, rayDistance, groundMask);

        return hitLeft.collider != null || hitCenter.collider != null || hitRight.collider != null;
    }

    public override void FixedUpdate()
    {
        
    }
}