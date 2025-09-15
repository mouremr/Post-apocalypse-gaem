using UnityEngine;

public class MantlingState : PlayerState
{
    private RaycastHit2D hipHit;
    private Vector2 headOrigin;
    private float facingDirection;
    
    private Vector2 targetMantlePosition;
    private Vector2 mantleBoost = new Vector2(0.5f, 2f);
    private float topLedgeY;
    private bool isMantleComplete = false;
    private float mantleTimer = 0f;
    private const float MANTLE_DURATION = 0.25f;

    public MantlingState(StateMachine stateMachine, RaycastHit2D hipHit, Vector2 headOrigin, float facingDirection) : base(stateMachine)
    {
        this.hipHit = hipHit;
        this.headOrigin = headOrigin;
        this.facingDirection = facingDirection;
    }

    public override void Enter()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        Debug.Log(facingDirection); 
        rb.AddForce(mantleBoost, ForceMode2D.Impulse);

        // Set animator parameters
        animator.SetBool("mantling", true);
        animator.SetBool("running", false);
        

        if (hipHit.collider != null)
        {
            topLedgeY = hipHit.collider.bounds.max.y;
        }

        targetMantlePosition = new Vector2(player.transform.position.x + (facingDirection * 0.6f), topLedgeY+1.3f); //+feet to hip height on the y
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        mantleTimer = 0f;
        isMantleComplete = false;
        
    }

    public override void Update()
    {
        
        // rb.constraints = RigidbodyConstraints2D.FreezeAll;   // Freeze completely

        Debug.DrawRay(targetMantlePosition, Vector2.up * 0.2f, Color.green);

        mantleTimer += Time.deltaTime;
                
        if (mantleTimer >= MANTLE_DURATION && !isMantleComplete)
        {
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.MovePosition(targetMantlePosition);

            isMantleComplete = true;
            animator.SetBool("mantling", false);
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            rb.AddForce(-1 * mantleBoost, ForceMode2D.Impulse);

            //Debug.Log("Mantle completed");
        }
        
        if (isMantleComplete)
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }
    }

}