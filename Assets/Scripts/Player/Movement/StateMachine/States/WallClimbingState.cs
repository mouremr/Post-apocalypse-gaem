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
    private bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Vector2 castDir = input.HorizontalInput >= 0 ? Vector2.right : Vector2.left;
        //you can only mantle if head ray detects nothing but hip ray detects an obstacle
        if (headHit.collider == null && hipHit.collider != null )
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
        float currentY=2f; // units per second

        Debug.Log("wall climbing state");

        if (!Input.GetKey(KeyCode.W))
        {
            currentY = -1.2f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            currentY = Mathf.Lerp(currentY, -2f, Time.deltaTime);
        }

        rb.linearVelocity = new Vector2(0f, currentY);


        if (wallExitTimer > 0f)
            wallExitTimer -= Time.deltaTime;

        if (wallExitTimer <= 0f && Mathf.Sign(input.HorizontalInput) != facingDirection && input.HorizontalInput != 0)
        {
            animator.SetBool("grounded", true);
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right; 
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength);

        if (canMantle(hipHit, headHit))
        {
            animator.SetBool("jumping", false);
            stateMachine.ChangeState(new MantlingState(stateMachine, hipHit, headOrigin));

        }


    }

    // private bool IsWalled()
    // {
    //     //get position of wallcheck obejct
    //     return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.05f, 0.5f) , 0, climbable);
    // }
    private bool IsWalled()
    {
        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        float rayLength = 0.3f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength);


        Debug.DrawRay(hipOrigin, castDir * rayLength, Color.red);
        Debug.DrawRay(headOrigin, castDir * rayLength, Color.blue);
        if (headHit.collider != null && hipHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
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
        
    }
}