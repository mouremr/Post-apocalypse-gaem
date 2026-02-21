using UnityEngine;
using UnityEngine.UIElements;

public class AttackState : PlayerState
{
    private float attackDuration;
    private float attackTimer;

    public AttackState(StateMachine stateMachine, PlayerStateConfig config) : base(stateMachine, config)
    {
        rb.linearVelocity = new Vector2(1 * input.HorizontalInput, rb.linearVelocity.y);
        
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length; //get length of attack animation
        animator.SetTrigger("attackTrigger");

    }

    // // Update is called once per frame
    public override void Update()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= attackDuration)
        {
            rb.linearVelocity = new Vector2(6 * input.HorizontalInput, rb.linearVelocity.y);
            animator.ResetTrigger("attackTrigger");
            stateMachine.ChangeState(new GroundedState(stateMachine, config));
        }
    }
}
