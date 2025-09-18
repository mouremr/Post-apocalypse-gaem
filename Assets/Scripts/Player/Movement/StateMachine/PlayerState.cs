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

    protected LayerMask climbable; // assign in inspector

    protected Transform wallCheck;

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
        climbable = LayerMask.GetMask("Climbable"); // Or whatever your layer is called

        wallCheck = player.transform.Find("WallCheck");


    }

    public virtual void Enter() { }
    public virtual void Update()
    { 

    }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}