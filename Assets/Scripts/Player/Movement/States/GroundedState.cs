using UnityEngine;

public class GroundedState : PlayerState
{
    private float moveSpeed = 7f;
    private InteractionDetector interactionDetector;
    private float groundCheckCooldown = 0.1f;
    private float groundCheckTimer = 0f;
    private float movementSmoothing = 10f;

    public GroundedState(StateMachine stateMachine) : base(stateMachine)
    {
        interactionDetector = stateMachine.InteractionDetector;
    }

    public override void Enter()
    {
        animator.SetBool("running", true);
        groundCheckTimer = groundCheckCooldown; // Start with cooldown
    }

    public override void Update()
    {
        groundCheckTimer -= Time.deltaTime;
        
        // Handle jumping - only check if cooldown is complete
        if (input.JumpPressed && groundCheckTimer <= 0f && IsGrounded())
        {
            stateMachine.ChangeState(new JumpingState(stateMachine, 5f));
        }
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
        float targetVelocityX = input.HorizontalInput * moveSpeed;
        float velocityDifferenceX = targetVelocityX - rb.linearVelocity.x;
        
        // Apply force to reach target velocity
        rb.AddForce(new Vector2(velocityDifferenceX * movementSmoothing * rb.mass * .1f, 0f), ForceMode2D.Force);
    }
}