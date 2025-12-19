using System;
using Unity.VisualScripting;
using UnityEngine;

public class WallPushOffState : PlayerState
{
    private float moveSpeed = 5f;
    private float minAirTime = 0.1f; // Minimum time before checking for ground
    private float airTimer = 0f;
    private float airControl = 5f; 
    private LayerMask climbableMask;

    private float wallDir;
    private float wallAttachLockout;
    private float wallAttachLockoutTime = 0.2f;

    private float pushX;
    private float pushY;
    public WallPushOffState(StateMachine stateMachine) : base(stateMachine)
    {
        climbableMask = LayerMask.GetMask("Climbable");


    }

    public override void Enter()
    {
        animator.SetBool("jumping", true);
        wallAttachLockout = wallAttachLockoutTime;

        if (!IsWalled(out wallDir))
        {
            wallDir = spriteRenderer.flipX ? -1f : 1f;
        }

        if(Math.Sign(Input.GetAxis("Horizontal")) == Math.Sign(-wallDir)){
            //push away from wall
            pushX = 20f * -wallDir; 
            pushY= 6f;
        } else {
            //push up
            pushX = 0;
            pushY = 8f;            
        }

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(new Vector2(pushX,pushY), ForceMode2D.Impulse);

        spriteRenderer.flipX = (wallDir == 1);


        airTimer = 0f; 
        rb.gravityScale = 1;
    }

    private bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Vector2 castDir = input.HorizontalInput >= 0 ? Vector2.right : Vector2.left;
        //you can only mantle if head ray detects nothing but hip ray detects an obstacle
        if (headHit.collider == null && hipHit.collider != null && hipHit.collider.CompareTag("Mantleable"))
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

        RaycastHit2D left = Physics2D.Raycast(hipOrigin, Vector2.left, rayLength,climbableMask);
        RaycastHit2D right = Physics2D.Raycast(hipOrigin, Vector2.right, rayLength,climbableMask);

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

        //Debug.Log("wall push off state");
        airTimer += Time.deltaTime;
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        float targetVelocityX = input.HorizontalInput * moveSpeed;
        float velocityDiff = targetVelocityX - rb.linearVelocity.x;

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = new Vector2(-wallDir, 0f); 
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength,climbableMask);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength,climbableMask);

        if (wallAttachLockout > 0f)
            wallAttachLockout -= Time.deltaTime;
        rb.AddForce(new Vector2(velocityDiff * airControl, 0f));

        if (input.JumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.AddForce(Vector2.down * rb.linearVelocity.y * 0.5f, ForceMode2D.Impulse);
        }

        if (rb.linearVelocity.y < -0.05f)
            rb.gravityScale = 1.3f;


        if (canMantle(hipHit, headHit))
         {
            animator.SetBool("jumping", false);
            stateMachine.ChangeState(new MantlingState(stateMachine, hipHit, headOrigin));
            return;
        }


        if (wallAttachLockout <= 0f && IsWalled(out _))
        {
            animator.SetBool("jumping", false);
            animator.SetBool("climbing", true);

            stateMachine.ChangeState(new WallClimbingState(stateMachine));
            return;


        }


        if (airTimer >= minAirTime && IsGrounded())
        {
            animator.SetBool("jumping", false);
            animator.SetBool("climbing", false);

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