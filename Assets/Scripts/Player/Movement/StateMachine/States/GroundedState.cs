using System;
using Unity.Mathematics;
using UnityEngine;

public class GroundedState : PlayerState
{
    private float moveSpeed = 5f;
    private float slideSpeed = 15f;
    private InteractionDetector interactionDetector;
    private float groundCheckCooldown = 0.1f;
    private float groundCheckTimer = 0f;
    private float movementSmoothing = 10f;

    //gracePeriod you can jump while being not grounded 
    private float gracePeriod = 0.2f; // makes this 0.2f 
    private float coyoteTimer = 0f; 
    float facingDirection;

    private float wallRegrabCooldown = 0.1f; // how long until you can re-grab wall
    private float wallRegrabTimer = 0f;


    public GroundedState(StateMachine stateMachine) : base(stateMachine)
    {
        facingDirection = spriteRenderer.flipX ? -1f : 1f;

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
        if (wallRegrabTimer > 0f)
            wallRegrabTimer -= Time.deltaTime;

        groundCheckTimer -= Time.deltaTime;

        if (IsGrounded())
        {
            coyoteTimer = gracePeriod;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        //check if possible to change state
        if ((input.JumpPressed && groundCheckTimer <= 0f && IsGrounded()) || (input.JumpPressed && groundCheckTimer <= 0f && coyoteTimer > 0f))
        {
            wallRegrabTimer = wallRegrabCooldown;


            stateMachine.ChangeState(new JumpingState(stateMachine, 5f, player.transform.position.y + 1f, slideSpeed));
        }

        if (!IsGrounded())
        {
            stateMachine.ChangeState(new JumpingState(stateMachine, 0f, player.transform.position.y, slideSpeed));
        }

        if (wallRegrabTimer <= 0f && IsWalled() && Mathf.Sign(input.HorizontalInput) == facingDirection)
        {
            wallRegrabTimer = wallRegrabCooldown;
            stateMachine.ChangeState(new WallClimbingState(stateMachine));
        }
        
        if (input.SlidePressed && IsGrounded() && !animator.GetBool("sliding"))
        {
            animator.SetBool("sliding", true);
            rb.AddForce(new Vector2(slideSpeed * input.HorizontalInput, 0), ForceMode2D.Impulse);
        }
        if (animator.GetBool("sliding") && math.abs(rb.linearVelocityX) <= moveSpeed + .5f)
        {
            Debug.Log("not sliding");
            animator.SetBool("sliding", false);
        }
    }

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
        if (headHit.collider != null && hipHit.collider != null && headHit.collider.gameObject.layer == LayerMask.NameToLayer("Climbable"))
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