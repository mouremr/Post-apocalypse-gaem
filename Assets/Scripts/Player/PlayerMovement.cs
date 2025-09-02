using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2(movementInput.x  * moveSpeed, rb.linearVelocityY);
        rb.linearVelocity = velocity;
    }
}