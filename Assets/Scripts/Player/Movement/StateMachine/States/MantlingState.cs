using UnityEngine;

public class MantlingState : PlayerState
{
    private RaycastHit2D hipHit;
    private Vector2 headOrigin;
    private float facingDirection;
    
    private Vector2 targetMantlePosition;

    private Vector2 intermediatePosition;
    private float topLedgeY;
    private float topLedgeX;

    private bool isMantleComplete = false;
    private float mantleTimer = 0f;
    private const float MANTLE_DURATION = 0.25f;

    private Vector2 oldSize;
    private Vector2 oldOffset;

    public MantlingState(StateMachine stateMachine, RaycastHit2D hipHit, Vector2 headOrigin, float facingDirection) : base(stateMachine)
    {
        this.hipHit = hipHit;
        this.headOrigin = headOrigin;
        this.facingDirection = facingDirection;
    }

    public override void Enter()
    {
        // Save original collider size/offset
        oldSize = boxCollider.size;
        oldOffset = boxCollider.offset;



        if (hipHit.collider != null)
        {
            topLedgeY = hipHit.collider.bounds.max.y; // top of the ledge
            topLedgeX = hipHit.point.x; // exact hit point
        }

        targetMantlePosition = new Vector2(player.transform.position.x+0.2f, topLedgeY + 1.3f);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        mantleTimer = 0f;
        isMantleComplete = false;
        camera.smoothTime = 0.3f;
        animator.SetBool("mantling", true);
        animator.SetBool("running", false);
    }

    public override void Update()
    {
        player.transform.position = new Vector2(topLedgeX, topLedgeY);
        
        Debug.DrawRay(targetMantlePosition, Vector2.up * 0.2f, Color.green);

        mantleTimer += Time.deltaTime;

        boxCollider.size = spriteRenderer.bounds.size;
        boxCollider.offset = spriteRenderer.bounds.center - player.transform.position;
       
        if (mantleTimer >= MANTLE_DURATION)
        {
            animator.SetBool("mantling", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            isMantleComplete = true;

        }
        
        if (isMantleComplete)
        {
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            camera.smoothTime = 0.2f;

            boxCollider.size = oldSize;
            boxCollider.offset = oldOffset;
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }
    }

}