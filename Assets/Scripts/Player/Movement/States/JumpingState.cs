using UnityEngine;

public class JumpingState : PlayerState
{
    private float moveSpeed = 5f;
    private float jumpForce;
    private float minAirTime = 0.1f; // Minimum time before checking for ground
    private float airTimer = 0f;

    private float facingDirection; //-1 if it facing right, 1 if facing left
    public JumpingState(StateMachine stateMachine, float specificForce) : base(stateMachine)
    {
        this.jumpForce = specificForce;

    }

    public override void Enter()
    {
        facingDirection = spriteRenderer.flipX ? -1f : 1f;

        animator.SetBool("jumping", true);
        rb.gravityScale = 1;
        rb.AddForce(new Vector2(jumpForce * input.HorizontalInput * 0.15f, jumpForce), ForceMode2D.Impulse); //slight push in moving direction    
        airTimer = 0f; // Reset timer
        rb.gravityScale = 1;
        facingDirection = Mathf.Sign(input.HorizontalInput);

    }

    private bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Debug.Log($"Hip hit: {(hipHit.collider != null ? hipHit.collider.name : "none")}");
        Debug.Log($"Head hit: {(headHit.collider != null ? headHit.collider.name : "none")}");

        Vector2 castDir = input.HorizontalInput >= 0 ? Vector2.right : Vector2.left;
        //you can only mantle if head ray detects nothing but hip ray detects an obstacle
        if (headHit.collider == null && hipHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public override void Update()
    {
        airTimer += Time.deltaTime;

        float targetVelocityX = input.HorizontalInput * moveSpeed;
        float velocityDiff = targetVelocityX - rb.linearVelocity.x;

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up *0.4f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 0.8f;

        Vector2 castDir = input.HorizontalInput >= 0 ? Vector2.right : Vector2.left;
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength);


        Debug.DrawRay(hipOrigin, castDir * rayLength, Color.red);
        Debug.DrawRay(headOrigin, castDir * rayLength, Color.blue);

        rb.AddForce(new Vector2(velocityDiff * 5f, 0f)); // "5f" = air acceleration factor

        if (input.JumpReleased && rb.linearVelocity.y > 0.1) //jujmp cut
        {
            rb.AddForce(new Vector2(0f, -rb.linearVelocity.y * 0.5f), ForceMode2D.Impulse);
        }
        
        if (canMantle(hipHit,headHit))
        {
            animator.SetBool("jumping", false);
            stateMachine.ChangeState(new MantlingState(stateMachine, hipHit, headOrigin,facingDirection));

        }

        if (rb.linearVelocity.y < -0.05f) //if falling, iuncrease gravity a little bit
        {
            rb.gravityScale = 1.3f;
        }


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