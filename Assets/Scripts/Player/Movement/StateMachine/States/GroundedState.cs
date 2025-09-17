using UnityEngine;

public class GroundedState : PlayerState
{
    private float moveSpeed = 5f;
    private InteractionDetector interactionDetector;
    private float groundCheckCooldown = 0.1f;
    private float groundCheckTimer = 0f;
    private float movementSmoothing = 10f;

    //gracePeriod you can jump while being not grounded 
    private float gracePeriod = 0.2f; // makes this 0.2f 

    private float coyoteTimer = 0f; 

    public GroundedState(StateMachine stateMachine) : base(stateMachine)
    {
        interactionDetector = stateMachine.InteractionDetector;
    }

    public override void Enter()
    {
        animator.SetBool("grounded", true);
        animator.SetBool("running", true);
        groundCheckTimer = groundCheckCooldown; // Start with cooldown
    }

    public override void Update()
    {
        groundCheckTimer -= Time.deltaTime;

        if (IsGrounded())
        {
            coyoteTimer = gracePeriod;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }


        // Handle jumping - only check if cooldown is complete
        if ((input.JumpPressed && groundCheckTimer <= 0f && IsGrounded()) || (input.JumpPressed && groundCheckTimer <= 0f && coyoteTimer > 0f))
        {

            stateMachine.ChangeState(new JumpingState(stateMachine, 5f,true));
        }

        if (!IsGrounded())
        {
            //Debug.Log("not grounded");

            stateMachine.ChangeState(new JumpingState(stateMachine, 0f,false));
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
        rb.AddForce(new Vector2(velocityDifferenceX * movementSmoothing * rb.mass * .75f, 0f), ForceMode2D.Force);

        if (Mathf.Abs(input.HorizontalInput) < 0.01)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x),0.5f); //reverse direction 
            amount *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
}