using UnityEngine;

public class WallClimbingState : PlayerState
{

    float yVelocity;

    float facingDirection;

    private float wallExitCooldown = 0.2f;
    private LayerMask climbableMask;
    private float wallExitTimer = 0f;
    private float dynoCooldownTimer = .6f;
    public WallClimbingState(StateMachine stateMachine) : base(stateMachine)
    {
        facingDirection = spriteRenderer.flipX ? -1f : 1f;
        wallExitTimer = wallExitCooldown; // start timer
        climbableMask = LayerMask.GetMask("Climbable");

    }

    public override void Enter()
    {
        animator.SetBool("climbing", true);

        int wallSide = GetWallSide();
        if (wallSide == -1)
            spriteRenderer.flipX = false; // face right
        else if (wallSide == 1)
            spriteRenderer.flipX = true;  // face left

        facingDirection = wallSide;

    }
    private int GetWallSide()
    {
        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        float rayLength = 0.4f;

        if (Physics2D.Raycast(hipOrigin, Vector2.left, rayLength,climbableMask))
            return 1; // wall on left

        if (Physics2D.Raycast(hipOrigin, Vector2.right, rayLength,climbableMask))
            return -1; // wall on right

        return 0;
    }
    private bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
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

        //Debug.Log("wall climbing state");
        animator.SetFloat("yVelocity",rb.linearVelocity.y);


        if  (input.JumpPressed && dynoCooldownTimer <= 0)
        {
            animator.SetBool("climbing", false);
            stateMachine.ChangeState(new WallPushOffState(stateMachine));
            return;

        }
        


        if (!(Input.GetAxis("Vertical") == 1))
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
        dynoCooldownTimer = Mathf.Max(0f, dynoCooldownTimer - Time.deltaTime);


        if (wallExitTimer <= 0f &&   input.HorizontalInput != 0 && Mathf.Sign(input.HorizontalInput) != facingDirection && IsGrounded())        {
            animator.SetBool("climbing", false);
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right; 
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength,climbableMask);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength,climbableMask);

        if (canMantle(hipHit, headHit))
        {
            animator.SetBool("climbing", false);
            stateMachine.ChangeState(new MantlingState(stateMachine, hipHit, headOrigin));
            return;
        }

        if (headHit.collider == null)
        {
            animator.SetBool("climbing", false);
            stateMachine.ChangeState(new JumpingState(stateMachine, 0,0,0));

        }

    }



    public override void FixedUpdate()
    {
        
    }
}