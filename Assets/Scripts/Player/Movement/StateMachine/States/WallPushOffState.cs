using System;
using Unity.VisualScripting;
using UnityEngine;

public class WallPushOffState : PlayerState
{
    private float moveSpeed = 5f;
    private float minAirTime = 0.1f; // Minimum time before checking for ground
    private float airTimer = 0f;
    private float airControl = 5f; 
    //private LayerMask climbableMask;

    private float wallDir;
    private float wallAttachLockout;
    private float wallAttachLockoutTime = 0.2f;

    private float pushX;
    private float pushY;
    public WallPushOffState(StateMachine stateMachine) : base(stateMachine)
    {

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

    public override void Update()
    {

        //Debug.Log("wall push off state");
        airTimer += Time.deltaTime;
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        float targetVelocityX = input.HorizontalInput * moveSpeed;
        float velocityDiff = targetVelocityX - rb.linearVelocity.x;


        if (wallAttachLockout > 0f)
            wallAttachLockout -= Time.deltaTime;
        rb.AddForce(new Vector2(velocityDiff * airControl, 0f));

        if (input.JumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.AddForce(Vector2.down * rb.linearVelocity.y * 0.5f, ForceMode2D.Impulse);
        }

        if (rb.linearVelocity.y < -0.05f)
            rb.gravityScale = 1.3f;


        if (canMantle())
         {
            animator.SetBool("jumping", false);
            stateMachine.ChangeState(new MantlingState(stateMachine));
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
}