using System;
using UnityEngine;

public class PlayerDamagedState : PlayerState
{
    private readonly Func<PlayerState> returnState;

    public PlayerDamagedState(StateMachine stateMachine, PlayerStateConfig config, Func<PlayerState> returnState) : base(stateMachine, config)
    {
        this.returnState = returnState;
        stateMachine.ModifyHealth(-2f);
    }

    public override void Enter()
    {
        animator.SetBool("Damaged", true);
        animator.Play("Damaged", 0, 0f);
    }

    public override void Exit()
    {
        animator.SetBool("Damaged", false);
    }

    public void DamageCompleted()
    {
        stateMachine.ChangeState(returnState());
    }
}