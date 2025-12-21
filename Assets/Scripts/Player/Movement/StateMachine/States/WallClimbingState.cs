using System;
using Unity.VisualScripting;
using UnityEngine;
public class WallClimbingState : PlayerState
{

    float yVelocity;

    float facingDirection;

    private float wallExitCooldown = 0.2f;
    //private LayerMask climbableMask;
    private float wallExitTimer = 0f;
    private float dynoCooldownTimer = .6f;
    private int dynoCost = 10;
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

    public override void Update()
    {
        float currentY=2f; // units per second

        animator.SetFloat("yVelocity",rb.linearVelocity.y);


        if  (input.JumpPressed && CanConsumeStamina(dynoCost)) // dyno up
        {
            animator.SetBool("climbing", false);
            float wallDir = spriteRenderer.flipX ? -1f : 1f;
            if(Math.Sign(Input.GetAxis("Horizontal")) == Math.Sign(-wallDir)){
                Debug.Log("push away from wall");
                float pushX = 6f * -wallDir; 
                float pushY= 6f;
                stateMachine.ChangeState(new JumpingState(stateMachine, new Vector2(pushX,pushY)));

            } else {
                float pushX = 0;
                float pushY = 8f;    
                stateMachine.ChangeState(new JumpingState(stateMachine, new Vector2(pushX,pushY)));
        
            }
            return;

        }


    
        if (!(Input.GetAxis("Vertical") == 1)) //fall or climb normally
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


        if (canMantle())
        {
            animator.SetBool("climbing", false);
            stateMachine.ChangeState(new MantlingState(stateMachine));
            return;
        }

    }




}