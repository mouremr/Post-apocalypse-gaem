using System;
using Unity.Mathematics;
using UnityEngine;


public class RollingState: PlayerState
{
    private float slideSpeed = 10f;
    private float moveSpeed;

    private float rollTimer;
    private float rollDuration=0.3f;
    public RollingState(StateMachine stateMachine,float moveSpeed) : base(stateMachine)
    {
        this.moveSpeed=moveSpeed;
    }

    public override void Enter()
    {
        animator.SetBool("rolling",true);
        rollTimer=rollDuration;
        rb.linearVelocity=Vector2.zero;
        
        float dir = spriteRenderer.flipX ? -1f : 1f;

        rb.AddForce(new Vector2(slideSpeed * dir, 0), ForceMode2D.Impulse);
        return;
    }
    public override void Update()
    {
        rollTimer -= Time.deltaTime;

        if (rollTimer <= 0f )
        {
            Debug.Log("end roll");
            animator.SetBool("rolling", false);
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }
    }
}