using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        rb.linearVelocity = transform.right * projectileSpeed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (collider.TryGetComponent(out StateMachine playerStateMachine))
        {
            playerStateMachine.TakeDamage(-projectileDamage);
        }
        Destroy(gameObject);
    }
}