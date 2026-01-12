using UnityEngine;

public class ScoutBehavior : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public float moveSpeed = 3f;
    public float attackRayLength = 0.5f; // player must be within 1 unit to begin attack

    public float detectRayLength=6; // 6 units of sight

    [SerializeField] private Transform detectRayOrigin;
    [SerializeField] private Transform attackRayOrigin;

    private LayerMask playerMask;

    private LayerMask climbableMask;

    private bool playerInSight;
    private bool isAttacking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMask = LayerMask.GetMask("Player"); 
        climbableMask = LayerMask.GetMask("Climbable"); 
    }

    void Update()
    {
        if (isAttacking){ //if attacking, finish attack 
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("chasePlayer", false);
            Debug.Log("I must finish attacking");
            return;
        }

        playerInSight = playerIsInSight();

        if(playerInSight){
            Debug.Log("I can see player!1");
            float facing = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;

            Vector2 direction = Vector2.right * facing;

            RaycastHit2D hit = Physics2D.Raycast(attackRayOrigin.position, direction, attackRayLength, playerMask);
            Debug.DrawRay(attackRayOrigin.position, direction * attackRayLength, Color.green);
            
            if (hit.collider==null) //if hit doesnt connect
            {
                anim.SetBool("chasePlayer", true); //chase
                anim.SetBool("attackPlayer", false);
                isAttacking = false; 

                rb.linearVelocity = new Vector2(facing * moveSpeed, rb.linearVelocity.y);
                GetComponent<SpriteRenderer>().flipX = facing < 0;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("chasePlayer", false);
                anim.SetBool("attackPlayer", true);
                isAttacking = true; 
            }
        }

        if (!playerInSight)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("chasePlayer", false);
            anim.SetBool("attackPlayer", false);
            Debug.Log("I cannot see player"); 
            return;
        }

    }

    bool playerIsInSight()
    {
        Vector2 facing = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(
            detectRayOrigin.position,
            facing,
            detectRayLength,
            playerMask | climbableMask
        );
        Debug.DrawRay(
           detectRayOrigin.position,              
            (Vector3)(facing * detectRayLength), 
            Color.blue
        );
        return hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0; //return true if something is hit, and the first object hit is the player
    }

    // Call this from an animation event at the end of the punch
    public void toggleCanAttack()
    {
        Debug.Log("animation ended, stop attacking");
        isAttacking = false;
        anim.SetBool("attackPlayer", false);
    }
}
