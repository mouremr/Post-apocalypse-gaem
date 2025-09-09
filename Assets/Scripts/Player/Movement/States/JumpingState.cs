using UnityEngine;

public class JumpingState : PlayerState
{
    private float moveSpeed = 5f;
    private float jumpForce;
    private float minAirTime = 0.1f; // Minimum time before checking for ground
    private float airTimer = 0f;

    public JumpingState(StateMachine stateMachine, float specificForce) : base(stateMachine)
    {
        this.jumpForce = specificForce;
    }

    public override void Enter()
    {
        animator.SetBool("jumping", true);
        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        airTimer = 0f; // Reset timer
    }

    public override void Update()
    {
        airTimer += Time.deltaTime;

        rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocity.y);

        if (input.JumpReleased && rb.linearVelocity.y > 0.1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        
        
        // Only check for ground after minimum air time to prevent immediate switching
        if (airTimer >= minAirTime && IsGrounded())
        {
            animator.SetBool("jumping", false);
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }
    }

    private bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        // Use multiple raycasts for better detection
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
}