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

    public GroundedState Grounded(bool resetLegs) => new(stateMachine, config);
    public JumpingState Jumping(Vector2 force) => new(stateMachine,config, force);
    public JumpingState Falling() => new(stateMachine, config, Vector2.zero);
    public RollingState Rolling(float moveSpeed) => new(stateMachine, config, moveSpeed);
    public WallClimbingState WallClimbing() => new(stateMachine, config);
    public MantlingState Mantling() => new(stateMachine, config);
    public PlayerAttackState LightAttack() => new(stateMachine, config, stateMachine.WeaponConfig.lightAttack);
    public PlayerAttackState HeavyAttack() => new(stateMachine, config, stateMachine.WeaponConfig.heavyAttack);
}
