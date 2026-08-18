using System;
using UnityEngine;

public class PlayerHangingState : PlayerState
{
    private static readonly int HangingHash = Animator.StringToHash("Hanging");

    public PlayerHangingState(StateMachine stateMachine, PlayerStateConfig config) : base(stateMachine, config)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Hanging");
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        animator.SetBool("Hanging", true);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Leaving Hanging");
        rb.gravityScale = config.gravityScale;
        animator.SetBool(HangingHash, false);
    }

    public override void Update()
    {
        base.Update();
        float wallDir = player.transform.localScale.x;
        // if (input.InteractPressed)
        // {
        //     Debug.Log("Leaving Hanging");
        //     animator.SetBool("Hanging", false);
            
        //     stateMachine.ChangeState(stateMachine.PlayerStates.WallClimbing());
        //     return;
        // }
        if(Math.Sign(Input.GetAxis("Horizontal")) == Math.Sign(-wallDir) && input.JumpPressed){
            animator.SetBool("climbing", false); //reset climbing to false to transition to grounded when landing
            rb.linearVelocity = Vector2.zero;
            float pushX = 2f * -wallDir; 
            float pushY= 5f;
            
            stateMachine.ChangeState(stateMachine.PlayerStates.Jumping(new Vector2(pushX,pushY)));
            return;
        }
    }
    

}
