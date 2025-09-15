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

        float feetToHipHeight = headOrigin.y - player.transform.position.y;

        targetMantlePosition = new Vector2(player.transform.position.x + (facingDirection * 0.75f), topLedgeY); //+feet to hip height on the y
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
        mantleTimer = 0f;
        isMantleComplete = false;
        
    }

    public override void Update()
    {
        mantleTimer += Time.deltaTime;
                
        // Automatically finish mantle after duration
        if (mantleTimer >= MANTLE_DURATION && !isMantleComplete)
        {
            FinishMantle();
        }
        
        // Transition to idle after mantle is complete
        if (isMantleComplete)
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }
    }

    private void FinishMantle()
    {
        rb.position = targetMantlePosition;

        isMantleComplete = true;
        animator.SetBool("mantling", false);

        Debug.Log("Mantle completed");
    }
}