using System;
using System.Runtime.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class StickBot : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private LayerMask playerMask;

    [SerializeField] private float patrolDistance = 5f;
    [SerializeField] private float detectionDistance = 3f;
    [SerializeField] private float moveSpeed = 3f;
    private Vector3 startPosition;
    private SpriteRenderer sprite;

    private bool patrol;
    private bool attacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMask = LayerMask.GetMask("Player");
        sprite = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        startPosition = transform.position;
        patrol = true;
    }

    void FixedUpdate()
    {
        
        if (patrol)
        {
            float distanceMoved = Vector3.Distance(startPosition, transform.position);
            bool movingAway = (transform.position.x > startPosition.x && moveSpeed > 0) ||
                      (transform.position.x < startPosition.x && moveSpeed < 0);
            if(distanceMoved >= patrolDistance && movingAway)
            {
                //FlipX();
                moveSpeed *= -1;
            }
            rb.linearVelocityX = moveSpeed;

            if (checkForPlayerLOS())
            {
                patrol = false;
                attacking = true;

            }
        }
        if (attacking)
        {
            
            // rb.linearVelocityX = 0;
            
            animator.SetBool("AttackState", true);
            if(checkForPlayerLOS() == false)
            {
                Debug.Log("back to patrolling");
                attacking = false;
                animator.SetBool("AttackState", false);                
                patrol = true;
            }
        }
        sprite.flipX = moveSpeed < 0;

    }

    private bool checkForPlayerLOS()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0,.5f,0);
        Vector2 facing = moveSpeed < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            facing,
            detectionDistance,
            playerMask
        );
        Debug.DrawRay(
        rayOrigin,              
            (Vector3)(facing * detectionDistance), 
            Color.blue
        );
        return hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0;
    }

    private void FlipX()
    {
        if (sprite.flipX) sprite.flipX = false;
        else sprite.flipX = true; 
    }


}
