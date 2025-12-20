using UnityEngine;

public class ScoutBehavior : MonoBehaviour
{
    private Animator anim;
    private GameObject target;
    private Rigidbody2D rb;

    public float moveSpeed = 3f;

    public Transform rayCast;
    public float rayCastLength = 2f;
    public LayerMask raycastMask;

    private RaycastHit2D hit;

    private bool playerInSight;
    private bool canAttack;
    private bool chasingPlayer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        raycastMask = LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        if (!playerInSight || target == null)
            return;

        ChasePlayer();

        if (canAttack)
            AttackPlayer();
                
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        Debug.Log("ENTER: " );

        if (trig.CompareTag("Player"))
        {
            target = trig.gameObject;
            playerInSight = true;
        }
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        Debug.Log("EXIT: ");

        if (trig.CompareTag("Player"))
        {
            playerInSight = false;
            chasingPlayer = false;
            canAttack = false;
            anim.SetBool("chasePlayer", false);
        }
    }

    void ChasePlayer()
    {
        hit = Physics2D.Raycast( rayCast.position,Vector2.left,rayCastLength,raycastMask );

        if (hit.collider != null)
        {
            chasingPlayer = true;
            anim.SetBool("chasePlayer", true);

            float distance = Vector2.Distance(
                transform.position,
                target.transform.position
            );
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

            canAttack = distance < 1.2f;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            chasingPlayer = false;
            canAttack = false;
            anim.   SetBool("chasePlayer", false);
        }
    }
    void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        chasingPlayer = false;
        anim.SetBool("chasePlayer", false);
        anim.SetBool("attackPlayer", true);

    }
    void toggleCanAttack() // called by animation event trigger at the end of the punch animation to stop attack
    {
        canAttack=false;
    }
}
