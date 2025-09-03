using UnityEngine;

public class GroundedState : PlayerState
{
    private float moveSpeed = 5f;
    private InteractionDetector interactionDetector;

    public GroundedState(StateMachine stateMachine) : base(stateMachine) 
    {
        interactionDetector = stateMachine.InteractionDetector;
    }

    public override void Enter()
    {
        //animator.SetBool("IsGrounded", true);
    }

    public override void Update()
    {
        // Handle jumping
        if (input.JumpPressed)
        {
            stateMachine.ChangeState(new JumpingState(stateMachine, 5f));
            Debug.Log("JUMP BUTTON PRESSED");
        }
    }

    public override void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocityY);
    }
}