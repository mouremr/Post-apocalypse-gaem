using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]  
public class PlayerSpriteFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerInput input;

    public Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInput>();
       // animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (input.HorizontalInput > 0){
            spriteRenderer.flipX = false;
            animator.SetBool("running",true);

        }
        else if (input.HorizontalInput < 0) {
            spriteRenderer.flipX = true;
            animator.SetBool("running",true);

        }
        else{
            animator.SetBool("running",false);
        }
    }
}