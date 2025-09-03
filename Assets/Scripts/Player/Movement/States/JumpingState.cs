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

    private string state = "jump"; // or "grounded", initialize somewhere

    public override void Update()
    {
        switch(state){
            case "jump":
                rb.linearVelocity = new Vector2(input.HorizontalInput * moveSpeed, rb.linearVelocity.y);
                if(IsGrounded()){
                    state="grounded";
                }
                break;
            case  "grounded":
                stateMachine.ChangeState(new GroundedState(stateMachine));
                break;
        }
    }
    private bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y); 

        return Physics2D.Raycast(origin, Vector2.down, 0.1f , LayerMask.GetMask("Ground"));
    }

    private bool IsNearLadder()
    {
        return Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Ladder"));
    }
}