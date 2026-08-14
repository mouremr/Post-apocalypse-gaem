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
        rb.bodyType = RigidbodyType2D.Kinematic; // or Dynamic if you want it to be pushed/interact with other physics
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // avoids tunneling through thin colliders at high speed
    }

    private void Start()
    {
        // fires horizontally in whatever direction the projectile is facing
        rb.linearVelocity = transform.right * projectileSpeed;
        Destroy(gameObject, lifetime); // cleanup if it never hits anything
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