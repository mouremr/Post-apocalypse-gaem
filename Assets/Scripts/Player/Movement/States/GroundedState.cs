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
        if (input.JumpPressed && IsGrounded())
        {
            stateMachine.ChangeState(new JumpingState(stateMachine, 5f));
        }
    }

    private bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y); 

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    public override void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocityY);
    }
}