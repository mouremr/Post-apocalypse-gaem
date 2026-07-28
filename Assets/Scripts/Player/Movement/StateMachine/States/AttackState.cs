using System;
using UnityEngine;

public class AttackState : PlayerState
{
    //private AnimatorStateInfo stateInfo;
    private float attackDuration;
    private float attackTimer = 0f;
    private string attackType;
    private float attackforce;
    private bool resetLegs;
    private BoxCollider2D weaponHitBox;

    public AttackState(StateMachine stateMachine, PlayerConfig config, String attack, float force, bool resetLegs) : base(stateMachine, config)
    {
        attackType = attack;
        attackforce = force;
    }

    public override void Enter()
    {
        animator.SetTrigger(attackType);
        weaponHitBox = stateMachine.WeaponHitBox;
        weaponHitBox.enabled = true;     
    }

    public void AttackCompleted()
    {
        if (stateMachine.CurrentState is AttackState)
        {
            stateMachine.ChangeState(stateMachine.States.Grounded(false));
        }
    }
    public override void Exit()
    {
        weaponHitBox.enabled = false;
    }

    public int DealDamage()
    {
        return 0;
    }

}