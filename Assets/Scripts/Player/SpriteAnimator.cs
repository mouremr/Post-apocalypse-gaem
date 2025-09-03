using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerSpriteFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public Animator animator;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalSpeed = rb.linearVelocity.x;


        if(Mathf.Abs(horizontalSpeed) > 0.1f) animator.SetBool("running", true);
        else if(Mathf.Abs(horizontalSpeed) < 0.1f) animator.SetBool("running", false);

        if (horizontalSpeed > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalSpeed < -0.1f)
        {
            spriteRenderer.flipX = true; 
        }
    }
}
