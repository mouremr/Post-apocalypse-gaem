// PlayerStateConfig.cs
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateConfig", menuName = "Game/Player State Config")]
public class PlayerStateConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    [Tooltip("grace period you can jump while being not grounded")]
    public float gracePeriod = .2f;
    
    
    [Header("Climbing")]
    public float climbSpeed = 4f;
    
    [Header("Rolling")]
    public float rollSpeed = 12f;
    public float rollDuration = 0.5f;
    public int rollCost = 10;
    
    [Header("Health & Stamina")]
    public float maxHealth = 10f;
    public float maxStamina = 20f;
    public float staminaRegenRate = 5f;
    public float staminaRegenDelay = 0.5f;

    [Header("Attacking")]
    public int lightAttackCost = 5;

    public int heavyAttackCost = 10;
}
