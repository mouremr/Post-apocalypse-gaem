using UnityEngine;

public class MantlingState : PlayerState
{
    private RaycastHit2D hipHit;
    private Vector2 headOrigin;
    private float facingDirection;
    
    private Vector2 targetMantlePosition;
    private float topLedgeY;
    private bool isMantleComplete = false;
    private float mantleTimer = 0f;
    private const float MANTLE_DURATION = 0.24999f;

    public MantlingState(StateMachine stateMachine, RaycastHit2D hipHit, Vector2 headOrigin, float facingDirection) : base(stateMachine)
    {
        this.hipHit = hipHit;
        this.headOrigin = headOrigin;
        this.facingDirection = facingDirection;
    }

    public override void Enter()
    {
                // Set animator parameters
        animator.SetBool("mantling", true);
        animator.SetBool("running", false);
        

        if (hipHit.collider != null)
        {
            topLedgeY = hipHit.collider.bounds.max.y;
        }

        rb.constraints = RigidbodyConstraints2D.FreezeAll;   // Freeze completely
        targetMantlePosition = new Vector2(player.transform.position.x + (facingDirection * 0.6f), topLedgeY+0.5f); //+feet to hip height on the y
        //rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
        mantleTimer = 0f;
        isMantleComplete = false;
        
    }

    public override void Update()
    {

        Debug.DrawRay(targetMantlePosition, Vector2.up * 0.2f, Color.green);

        mantleTimer += Time.deltaTime;
                
        if (mantleTimer >= MANTLE_DURATION && !isMantleComplete)
        {
            rb.MovePosition(targetMantlePosition);

            isMantleComplete = true;
            animator.SetBool("mantling", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; 


            Debug.Log("Mantle completed");
        }
        
        if (isMantleComplete)
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }
    }

}