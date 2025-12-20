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

    private LayerMask manteableMask;

    public JumpingState(StateMachine stateMachine, float jumpForce, float yval, float slideSpeed) : base(stateMachine)
    {
        this.jumpForce = jumpForce;
        this.yval = yval;
        this.slideSpeed = slideSpeed;
        manteableMask = LayerMask.GetMask("Mantleable");
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
        if (hipHit.collider != null && hipHit.collider.CompareTag("Mantleable") && headHit.collider == null)    
        {
            Debug.Log("can mantle");

            return true;

        }
        else
        {
            Debug.Log("cant mantle");
            return false;
        }
        
    }


    public override void Update()
    {

        input.ConsumeRoll();//kill buffered rolls

        //Debug.Log("jumping state");
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

}