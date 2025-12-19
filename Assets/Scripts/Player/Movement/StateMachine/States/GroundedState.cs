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
    private float rollCheckCooldown = .6f;
    private float rollCheckTimer = 0f;
    private float movementSmoothing = 10f;

    //gracePeriod you can jump while being not grounded 
    private float gracePeriod = 0.2f;
    private float coyoteTimer = 0f; 
    float facingDirection;

    private float wallRegrabCooldown = 0.1f; // how long until you can re-grab wall
    private float wallRegrabTimer = 0f;

    private LayerMask climbableMask;


    public GroundedState(StateMachine stateMachine) : base(stateMachine)
    {
        facingDirection = spriteRenderer.flipX ? -1f : 1f;

        interactionDetector = stateMachine.InteractionDetector;
        climbableMask = LayerMask.GetMask("Climbable");
    }

    public override void Enter()
    {
        animator.SetBool("grounded", true);
        animator.SetBool("running", true);
        input.ConsumeRoll();

        groundCheckTimer = groundCheckCooldown; // Start with cooldown
        rollCheckTimer = rollCheckCooldown;

        if (animator.GetBool("rolling"))
            return;
    }

    public override void Update()
    {   
        facingDirection = spriteRenderer.flipX ? -1f : 1f;

        //Debug.Log("grounded state");
        if (wallRegrabTimer > 0f)
            wallRegrabTimer -= Time.deltaTime;

        groundCheckTimer = Mathf.Max(0f, groundCheckTimer - Time.deltaTime);
        rollCheckTimer = Mathf.Max(0f, rollCheckTimer - Time.deltaTime);
        

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

        if (IsGrounded())
        {
            coyoteTimer = gracePeriod;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            animator.SetBool("grounded", false);
        }

        //check if possible to change state
        if (wallRegrabTimer <= 0f && IsWalled(out float mrow) && !IsGrounded() && Mathf.Abs(input.HorizontalInput) > 0.01f)
        {
            wallRegrabTimer = wallRegrabCooldown;
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);
            Debug.Log("entering wall cloimbing state from fall or standing");

            stateMachine.ChangeState(new WallClimbingState(stateMachine));
            return;
        }else  if ((input.JumpPressed && groundCheckTimer <= 0f && IsGrounded()) || (input.JumpPressed && groundCheckTimer <= 0f && coyoteTimer > 0f))
        {
            wallRegrabTimer = wallRegrabCooldown;
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);

            stateMachine.ChangeState(new JumpingState(stateMachine, 5f, player.transform.position.y + 1f, slideSpeed));
            return;
        }
        else if(!IsGrounded()){
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);

            stateMachine.ChangeState(new JumpingState(stateMachine, 0, player.transform.position.y + 1f, slideSpeed));
        }
        else if (input.RollPressed && IsGrounded() && rollCheckTimer <= 0f)
        {   
            rollCheckTimer = rollCheckCooldown;
            animator.SetBool("grounded", false);
            Debug.Log("moving to rollingstate");
            stateMachine.ChangeState(new RollingState(stateMachine,moveSpeed));
        }
        else
        {
            animator.SetBool("running", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }


        if (Mathf.Abs(input.HorizontalInput) > 0.01f)
        {
            spriteRenderer.flipX = input.HorizontalInput < 0;
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

        RaycastHit2D hitGround = Physics2D.Raycast(originCenter, Vector2.down, rayDistance, groundMask);
        RaycastHit2D hitClimbable = Physics2D.Raycast(originCenter, Vector2.down, rayDistance, climbableMask);

        return hitGround.collider != null || hitClimbable.collider != null ;
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