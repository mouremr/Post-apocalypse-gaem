using UnityEngine;

public class PlayerStateFactory
{
    private readonly StateMachine stateMachine;
    private readonly PlayerStateConfig config;

    public PlayerStateFactory(StateMachine stateMachine, PlayerStateConfig config)
    {
        this.stateMachine = stateMachine;
        this.config = config;
    }

    public GroundedState Grounded(bool resetLegs) => new(stateMachine, config, resetLegs);
    public JumpingState Jumping(Vector2 force) => new(stateMachine, force, config);
    public JumpingState Falling() => new(stateMachine, Vector2.zero, config);
    public RollingState Rolling(float moveSpeed) => new(stateMachine, moveSpeed, config);
    public WallClimbingState WallClimbing() => new(stateMachine, config);
    public MantlingState Mantling() => new(stateMachine, config);
    public AttackState LightAttack() => new(stateMachine, config, "lightAttack", config.moveSpeed, false);
    public AttackState HeavyAttack() => new(stateMachine, config, "heavyAttack", 0f, true);
}
