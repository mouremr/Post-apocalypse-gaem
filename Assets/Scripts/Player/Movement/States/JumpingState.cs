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
    }


    public override void Update()
    {
  
        rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocity.y);
        if(IsGrounded()){
            stateMachine.ChangeState(new GroundedState(stateMachine));
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

    private bool IsNearLadder()
    {
        return Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Ladder"));
    }
}