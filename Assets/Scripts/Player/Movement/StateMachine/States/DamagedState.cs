using System;
using UnityEngine;

public class PlayerDamagedState : PlayerState
{
    private readonly Func<PlayerState> returnState;

    public PlayerDamagedState(StateMachine stateMachine, PlayerStateConfig config, Func<PlayerState> returnState, float amount) : base(stateMachine, config)
    {
        this.returnState = returnState;
        stateMachine.ModifyHealth(amount);
    }

    public override void Enter()
    {
        animator.SetBool("Damaged", true);
        animator.Play("Damaged", 0, 0f); //layer 0 = base layer in animator 
    }

    public override void Exit()
    {
        animator.SetBool("Damaged", false);
    }

    public void DamageCompleted()
    {
        stateMachine.ChangeState(returnState());
    }
    
    //ignore hits while already taking damage
    public override void TakeDamage(float amount) { }
}