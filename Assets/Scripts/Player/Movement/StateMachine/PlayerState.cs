using UnityEngine;

public abstract class PlayerState
{
    protected StateMachine stateMachine;
    protected GameObject player;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected PlayerInput input;
    protected SpriteRenderer spriteRenderer;

    protected BoxCollider2D boxCollider;

    protected CameraFollow camera;

    protected LayerMask climbableMask; // assign in inspector

    //protected Transform wallCheck;
    protected LayerMask groundMask; // assign in inspector

    public PlayerState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        player = stateMachine.gameObject;
        rb = player.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();
        input = player.GetComponent<PlayerInput>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        boxCollider = player.GetComponent<BoxCollider2D>();
        camera = Camera.main.GetComponent<CameraFollow>();
        groundMask = LayerMask.GetMask("Ground");
        climbableMask = LayerMask.GetMask("Climbable");

    }

    public virtual void Enter() { }
    public virtual void Update()
    { 

    }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }



    protected bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        Vector2 originLeft = new Vector2(bounds.min.x + 0.1f, bounds.min.y);
        Vector2 originCenter = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 originRight = new Vector2(bounds.max.x - 0.1f, bounds.min.y);

        float rayDistance = 0.1f;

        RaycastHit2D hitGrounded = Physics2D.Raycast(originCenter, Vector2.down, rayDistance, groundMask);
        RaycastHit2D hitClimbable = Physics2D.Raycast(originCenter, Vector2.down, rayDistance, climbableMask);

        return hitGrounded.collider != null || hitClimbable.collider != null;
    }

    protected bool IsWalled(out float direction)
    {
        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        float rayLength = 0.4f;

        RaycastHit2D left = Physics2D.Raycast(hipOrigin, Vector2.left, rayLength,climbableMask);
        RaycastHit2D right = Physics2D.Raycast(hipOrigin, Vector2.right, rayLength,climbableMask);

        Debug.DrawRay(hipOrigin, Vector2.left * rayLength, Color.red);
        Debug.DrawRay(hipOrigin, Vector2.right * rayLength, Color.blue);

        if (left.collider != null)
        {
            direction = -1;
            return true;
        }

        if (right.collider != null)
        {
            direction = 1;
            return true;
        }

        direction = 0;
        return false;
    }
    protected bool canMantle(RaycastHit2D hipHit, RaycastHit2D headHit)
    {
        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        //you can only mantle if head ray detects nothing but hip ray detects an obstacle
        if (hipHit.collider != null && hipHit.collider.CompareTag("Mantleable") && headHit.collider == null)    
        {
            Debug.Log("can mantle");

            return true;

        }
        else
        {
            Debug.Log("cant mantle");
            return false;
        }
        
    }
}