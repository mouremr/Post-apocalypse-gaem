using System;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    //private AnimatorStateInfo stateInfo;
    private AttackData attackData;
    private BoxCollider2D weaponHitBox;

    public PlayerAttackState(StateMachine stateMachine, PlayerStateConfig config, AttackData attackData) : base(stateMachine, config)
    {
        this.attackData = attackData;
    }

    public override void Enter()
    {
        animator.SetTrigger(attackData.attackTrigger);
        weaponHitBox = stateMachine.WeaponHitBox;
        weaponHitBox.enabled = true;     
    }

    public void AttackCompleted()
    {
        if (stateMachine.CurrentState is PlayerAttackState)
        {
            stateMachine.ChangeState(stateMachine.PlayerStates.Grounded(false));
        }
    }
    public override void Exit()
    {
        weaponHitBox.enabled = false;
    }

}