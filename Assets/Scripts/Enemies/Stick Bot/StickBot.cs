using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float patrolDistance = 5f;
    [SerializeField] private float detectionDistance = 3f;
    [SerializeField] private float backupDetectionDistance;
    [SerializeField] private float backupDuration = 0.5f; // How long it backs up before re-evaluating
    [SerializeField] private float moveSpeed = 3f;
    private Vector3 startPosition;
    private SpriteRenderer sprite;

    private bool patrol;
    private bool attacking = false;
    private float backupTimer = 0f;

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
            // Go back to starting position
            float distanceMoved = Vector3.Distance(startPosition, transform.position);
            bool movingAway = (transform.position.x > startPosition.x && moveSpeed > 0) ||
                            (transform.position.x < startPosition.x && moveSpeed < 0);
            
            if (distanceMoved >= patrolDistance && movingAway)
            {
                moveSpeed *= -1;
            }
            
            rb.linearVelocityX = moveSpeed;

            if (CheckForPlayerLOS())
            {
                patrol = false;
                attacking = true;
                //animator.SetBool("Walking", false);
            }
        }

        if (attacking)
        {
            if (backupTimer > 0)
            {
                backupTimer -= Time.fixedDeltaTime;
            }

            //trigger backup if player gets too close
            if (IsPlayerClose())
            {
                backupTimer = backupDuration; //reset timer
            }

            //stay until timer resolved
            if (backupTimer > 0)
            {
                animator.ResetTrigger("Attacking");
                animator.SetBool("Backup", true);
                rb.linearVelocityX = moveSpeed * -.5f;
            } 
            else
            {
                animator.SetBool("Backup", false);
                animator.SetTrigger("Attacking");
                //rb.linearVelocityX = 0; // Stop moving while shooting, or move normal speed
            
                
            }
            if(!CheckForPlayerLOS())
            {
                attacking = false;
                animator.ResetTrigger("Attacking");
                animator.SetTrigger("Walking");                
                patrol = true;
            }
        }
        
        sprite.flipX = moveSpeed < 0;
    }

    private bool IsPlayerClose()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0,.25f,0);
        Vector2 facing = moveSpeed < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            facing,
            backupDetectionDistance,
            playerMask
        );
        Debug.DrawRay(
        rayOrigin,              
            (Vector3)(facing * backupDetectionDistance), 
            Color.red
        );
        return hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0;
    }

    private bool CheckForPlayerLOS()
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
