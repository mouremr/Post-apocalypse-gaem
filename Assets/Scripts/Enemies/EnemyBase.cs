using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    [Header("Shared movement/detection (same across all states)")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionDistance = 3f;

    public float MoveSpeed => moveSpeed;
    public float CurrentDirection { get; private set; } = 1f;
    public void SetDirection(float dir) => CurrentDirection = Mathf.Sign(dir);
    public float DetectionDistance => detectionDistance;

    [SerializeField] private EnemyState startingState;

    private EnemyState[] states;
    private EnemyState current;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        states = GetComponents<EnemyState>();
        foreach (var s in states) s.Init(this);
    }

    protected virtual void Start()
    {
        if (startingState != null)
            ChangeState(startingState);
        else if (states.Length > 0)
            ChangeState(states[0]);
    }

    protected virtual void Update() => current?.Tick();
    protected virtual void FixedUpdate() => current?.FixedTick();

    public void ChangeState(EnemyState next)
    {
        if (next == current) return;
        current?.Exit();
        current = next;
        current.Enter();
    }

    public void GoTo<T>() where T : EnemyState
    {
        foreach (var state in states)
        {
            if (state is T)
            {
                ChangeState(state);
                return;
            }
        }
        Debug.LogWarning($"{name} has no state of type {typeof(T).Name}");
    }

    public virtual void TakeDamage(Weapon weapon)
    {
        currentHealth -= weapon.DealDamage();

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Weapon"))
        {
            Weapon weapon = collider.gameObject.GetComponent<Weapon>();
            TakeDamage(weapon);
        }
    }
}