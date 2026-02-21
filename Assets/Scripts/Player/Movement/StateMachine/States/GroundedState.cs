using System;
using Unity.Mathematics;
using UnityEngine;

public class GroundedState : PlayerState
{
    private float moveSpeed;
    //private InteractionDetector interactionDetector;
    private float groundCheckCooldown = 0.1f;
    private float groundCheckTimer = 0f;
    private float rollCheckCooldown = .6f;
    private float rollCheckTimer = 0f;
    private float movementSmoothing = 10f;

    //gracePeriod you can jump while being not grounded 
    private float gracePeriod;
    private float coyoteTimer = 0f; 
    //float facingDirection;

    private float wallRegrabCooldown = 0.1f; // how long until you can re-grab wall
    private float wallRegrabTimer = 0f;

    private int rollCost;
    private int attackCost;



    public GroundedState(StateMachine stateMachine, PlayerStateConfig config) : base(stateMachine, config)
    {
        moveSpeed = config.moveSpeed;
        gracePeriod = config.gracePeriod;
        rollCost = config.rollCost;
        attackCost = config.attackCost;
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
        base.Update();
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
            //wallclimbing state
            wallRegrabTimer = wallRegrabCooldown;
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);
            //Debug.Log("entering wall cloimbing state from fall or standing");

            stateMachine.ChangeState(new WallClimbingState(stateMachine, config));
            return;
        }else  if ((input.JumpPressed && groundCheckTimer <= 0f && IsGrounded()) || (input.JumpPressed && groundCheckTimer <= 0f && coyoteTimer > 0f))
        {
            //jumping state
            wallRegrabTimer = wallRegrabCooldown;
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);

            stateMachine.ChangeState(new JumpingState(stateMachine, new Vector2(0f,5f), config));
            return;
        }
        else if(!IsGrounded()){
            //falling if not on ground
            animator.SetBool("grounded", false);
            animator.SetBool("running", false);

            stateMachine.ChangeState(new JumpingState(stateMachine, new Vector2(0,0f), config));
        }
        else if (input.RollPressed && IsGrounded() && ConsumeStamina(rollCost))
        {   
            //roll state
            animator.SetBool("grounded", false);
            stateMachine.ChangeState(new RollingState(stateMachine,moveSpeed, config));
        }
        else if (input.AttackPressed && ConsumeStamina(attackCost))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("running", false);
            stateMachine.ChangeState(new AttackState(stateMachine, config));
        }
        else
        {
            //otherwise move to running
            animator.SetBool("running", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }


        if (Mathf.Abs(input.HorizontalInput) > 0.01f)
        {
            spriteRenderer.flipX = input.HorizontalInput < 0;
        }

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