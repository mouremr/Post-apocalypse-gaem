using UnityEngine;

public class MantlingState : PlayerState
{
    private RaycastHit2D hipHit;
    private Vector2 headOrigin;
    private float facingDirection;
    private LayerMask climbableMask;

    private Vector2 targetMantlePosition;

    private Vector2 intermediatePosition;
    private float topLedgeY;
    private float topLedgeX;

    private bool isMantleComplete = false;
    private float mantleTimer = 0f;
    private const float MANTLE_DURATION = 0.25f;

    private Vector2 oldSize;
    private Vector2 oldOffset;

    public MantlingState(StateMachine stateMachine) : base(stateMachine)
    {
        this.hipHit = hipHit;
        this.headOrigin = headOrigin;
        climbableMask=LayerMask.GetMask("Climbable");
    }

    public override void Enter()
    {

        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength);
        
        // Save original collider size/offset
        oldSize = boxCollider.size;
        oldOffset = boxCollider.offset;
        animator.SetBool("mantling", true);

        if (hipHit.collider != null )
        {
            topLedgeY = hipHit.collider.bounds.max.y; // top of the ledge
            topLedgeX = hipHit.point.x; // exact hit point
        }

        targetMantlePosition = new Vector2(player.transform.position.x+0.2f, topLedgeY);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        mantleTimer = 0f;
        isMantleComplete = false;
        camera.smoothTime = 0.3f;
    }

    public override void Update()
    {
        if (!isMantleComplete)
        {
            player.transform.position = new Vector2(topLedgeX, topLedgeY);
        }        
        Debug.DrawRay(targetMantlePosition, Vector2.up * 0.2f, Color.green);

        mantleTimer += Time.deltaTime;

        boxCollider.size = spriteRenderer.bounds.size;
        boxCollider.offset = spriteRenderer.bounds.center - player.transform.position;
       
        if (mantleTimer >= MANTLE_DURATION)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            isMantleComplete = true;
            animator.SetBool("mantling", false);


        }
        
        if (isMantleComplete)
        {
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            camera.smoothTime = 0.2f;
            animator.SetBool("mantling", false);

            boxCollider.size = oldSize;
            boxCollider.offset = oldOffset;
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }
    }

}