using System;
using UnityEngine;

public class AttackState : PlayerState
{
    //private AnimatorStateInfo stateInfo;
    private float attackDuration;
    private float attackTimer = 0f;
    private string attackType;
    private float attackforce;

    public AttackState(StateMachine stateMachine, PlayerStateConfig config, String attack, float force) : base(stateMachine, config)
    {
        attackType = attack;
        attackforce = force;
    }

    public override void Update()
    {
        //rb.linearVelocity = new Vector2(rb.linearVelocityX * input.HorizontalInput, rb.linearVelocityY);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(attackforce * input.HorizontalInput, 0), ForceMode2D.Impulse);
        // get attack animation length
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length; 
        attackTimer += Time.deltaTime;

        

        if(attackTimer >= attackDuration)
        {
            animator.SetBool(attackType, false);
            stateMachine.ChangeState(new GroundedState(stateMachine, config));
        }
    }

    public override void Enter()
    {
        animator.SetBool(attackType, true);        
    }

}