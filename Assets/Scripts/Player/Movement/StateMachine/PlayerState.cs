using System;
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
    protected static float maxStamina = 20f;

    protected static float staminaRegenTimer = 0f;
    protected static float staminaRegenDelay = .5f;
    protected static float staminaRegenRate = 5f;


    protected static float currentStamina = maxStamina;
    

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
        RegenStamina();
    }

    private void RegenStamina()
    {
        if (currentStamina >= maxStamina)
            return;

        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    public virtual void FixedUpdate() { }
    public virtual void Exit() { }



    protected bool IsGrounded()
    {
        Collider2D col = player.GetComponent<Collider2D>();
        Bounds bounds = col.bounds;

        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, 0.1f);
        Vector2 boxCenter = bounds.center - new Vector3(0, bounds.extents.y, 0);
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.down,
            .1f,
            groundMask | climbableMask
        );

        Debug.DrawRay(boxCenter, Vector2.down * .1f, Color.green);

        
        return hit.collider != null;
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
    protected bool canMantle()
    {
        Vector2 hipOrigin = (Vector2)player.transform.position + Vector2.up * 1f;
        Vector2 headOrigin = hipOrigin + Vector2.up * 1f;

        Vector2 castDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        float rayLength = 0.5f;
        RaycastHit2D hipHit = Physics2D.Raycast(hipOrigin, castDir, rayLength,climbableMask);
        RaycastHit2D headHit = Physics2D.Raycast(headOrigin, castDir, rayLength,climbableMask);

        Debug.DrawRay(hipOrigin, castDir * rayLength, Color.red);
        Debug.DrawRay(headOrigin, castDir * rayLength, Color.blue);


        if (hipHit.collider != null && hipHit.collider.CompareTag("Mantleable") && headHit.collider == null)    
        {
            return true;

        }
        else
        {
            return false;
        }
        
    }
    public bool CanConsumeStamina(int cost)
    {
        if (currentStamina >= cost)
        {
            currentStamina -= cost;
            return true;
        }
        return false;
    }

    public static float GetCurrentStamina()
    {
        return currentStamina;
    }

    public static float GetMaxStamina()
    {
        return maxStamina;
    }

}