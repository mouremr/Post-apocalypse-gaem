using UnityEngine;

public abstract class PlayerState
{
    protected StateMachine stateMachine;
    protected GameObject player;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected PlayerInput input;

    public PlayerState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        player = stateMachine.gameObject;
        rb = player.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();
        input = player.GetComponent<PlayerInput>();
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}