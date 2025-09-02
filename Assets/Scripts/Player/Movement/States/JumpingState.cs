using UnityEngine;

public class JumpingState : PlayerState
{
    private float moveSpeed = 5f;
    private float jumpForce;


    public JumpingState(StateMachine stateMachine, float specificForce) : base(stateMachine)
    {
        this.jumpForce = specificForce;    
    }

    public override void Enter()
    {
        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //animator.SetBool("IsGrounded", false);
    }

    public override void Update()
    {
        // Check if player has landed
        if (Mathf.Abs(rb.linearVelocity.y) < 0.1f && IsGrounded())
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Air control
        rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        // Implement ground detection
        return Physics2D.Raycast(player.transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Ground"));
    }

    private bool IsNearLadder()
    {
        return Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Ladder"));
    }
}