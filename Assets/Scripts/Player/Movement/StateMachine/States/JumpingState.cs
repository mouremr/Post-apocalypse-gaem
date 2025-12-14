using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : PlayerState
{
    private float moveSpeed = 5f;
    private float jumpForce;
    private float minAirTime = 0.1f; // Minimum time before checking for ground
    private float airTimer = 0f;
    private float airControl = 5f; 
    private float yval;
    private float slideSpeed = 0;
    private float rollHeightCutoff = 4f;
    public JumpingState(StateMachine stateMachine, float jumpForce, float yval, float slideSpeed) : base(stateMachine)
    {
        this.jumpForce = jumpForce;
        this.yval = yval;
        this.slideSpeed = slideSpeed;
    }

    public override void Enter()
    {
        animator.SetBool("jumping", true);
        input.ConsumeRoll();

        //rb.gravityScale = 1;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.AddForce(new Vector2(jumpForce * input.HorizontalInput * 0.15f, jumpForce), ForceMode2D.Impulse);
        airTimer = 0f; // Reset timer
        rb.gravityScale = 1;
    }

    private bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
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

    private bool IsWalled(out float direction)
    {
        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        float rayLength = 0.4f;

        RaycastHit2D left = Physics2D.Raycast(hipOrigin, Vector2.left, rayLength);
        RaycastHit2D right = Physics2D.Raycast(hipOrigin, Vector2.right, rayLength);

        Debug.DrawRay(hipOrigin, Vector2.left * rayLength, Color.red);
        Debug.DrawRay(hipOrigin, Vector2.right * rayLength, Color.blue);

        if (left.collider != null)
        {
            direction = -1;
            return true;
        }

        if (right.collider != null)
        {
            direction = 1;
            return true;
        }

        direction = 0;
        return false;
    }

    public override void Update()
    {

        input.ConsumeRoll();//kill buffered rolls

        Debug.Log("jumping state");
        airTimer += Time.deltaTime;
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        float targetVelocityX = input.HorizontalInput * moveSpeed;
        float velocityDiff = targetVelocityX - rb.linearVelocity.x;

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength);


        Debug.DrawRay(hipOrigin, castDir * rayLength, Color.red);
        Debug.DrawRay(headOrigin, castDir * rayLength, Color.blue);

        rb.AddForce(new Vector2(velocityDiff * airControl, 0f));


        if (input.JumpReleased && rb.linearVelocity.y > 0.1) //jump cut
        {
            rb.AddForce(new Vector2(0f, -rb.linearVelocity.y * 0.5f), ForceMode2D.Impulse);
        }
        if (rb.linearVelocity.y < -0.05f) //if falling, iuncrease gravity a little bit
        {
            rb.gravityScale = 1.3f;
        }


        if (canMantle(hipHit, headHit))
        {
            animator.SetBool("jumping", false);
            animator.SetBool("climbing", false);
            animator.SetBool("mantling", true);

            Debug.Log("moving into mantling from jump state");
            stateMachine.ChangeState(new MantlingState(stateMachine, hipHit, headOrigin));
            return;
        }
        else if (IsWalled(out float wallDir))
        {
            animator.SetBool("jumping", false);

            stateMachine.ChangeState(new WallClimbingState(stateMachine));

            return;
        }

        else if (airTimer >= minAirTime && IsGrounded())
        {
            animator.SetBool("jumping", false);
            animator.SetBool("grounded", true);
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }


        if (rb.linearVelocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
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