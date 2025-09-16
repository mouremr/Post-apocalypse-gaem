using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FlipSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public Animator animator;
    public BoxCollider2D boxCollider;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    private void Update()
    {


        float horizontalSpeed = rb.linearVelocity.x;

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        animator.SetBool("running", Mathf.Abs(horizontalSpeed) > 0.1f);

        if (horizontalSpeed > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalSpeed < -0.1f)
        {
            spriteRenderer.flipX = true;
        }

        // animator.SetBool("jumping", rb.linearVelocity.y > 0.1f || !IsGrounded());

        
    }
}
