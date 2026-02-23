
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [SerializeField] private PlayerStateConfig stateConfig;
    [SerializeField] private LayerMask climbable;

    private PlayerState _currentState;
    private InteractionDetector interactionDetector;


    private float currentStamina;
    private float maxStamina;
    private float currentHealth;
    private float maxHealth;
    
    // Stamina regeneration timers
    private float staminaRegenTimer = 0f;
    private float staminaRegenDelay;
    private float staminaRegenRate;

    public PlayerState CurrentState => _currentState;
    public InteractionDetector InteractionDetector => interactionDetector;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    //for the future maybe move health away from movement states?
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        maxHealth = stateConfig.maxHealth;
        currentHealth = maxHealth;
        maxStamina = stateConfig.maxStamina;
        currentStamina = maxStamina;
        staminaRegenRate = stateConfig.staminaRegenRate;
        staminaRegenDelay = stateConfig.staminaRegenDelay;        
    }

    private void Start()
    {
        interactionDetector = GetComponent<InteractionDetector>();
        ChangeState(new GroundedState(this, stateConfig));
    }

    private void Update()
    {
        _currentState?.Update();
        RegenStamina();

        // Handle global interaction input
        if (Input.GetKeyDown(KeyCode.E) && interactionDetector.HasInteractible)
        {
            interactionDetector.CurrentInteractible.Interact(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    private void RegenStamina()
    {
        if (currentStamina >= maxStamina)
            return;

        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }
    public bool ConsumeStamina(int cost)
    {
        if (currentStamina >= cost)
        {
            currentStamina -= cost;
            return true;
        }
        return false;
    }
    
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}