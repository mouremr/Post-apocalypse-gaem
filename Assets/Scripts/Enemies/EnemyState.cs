using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyBase enemy;
    protected bool attacking = false;
    protected float backupTimer = 0f;


    protected float moveSpeed;
    protected float detectionDistance;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite; //switch to transform.localscale *= -1
    protected LayerMask playerMask;
    protected Vector3 startPosition;

    public virtual void Init(EnemyBase owner)
    {
        enemy = owner;
        moveSpeed = enemy.MoveSpeed;
        detectionDistance = enemy.DetectionDistance;
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerMask = LayerMask.GetMask("Player");
        startPosition = transform.position;
    }


    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }



    protected bool CheckForPlayerLOS()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, .5f, 0);
        Vector2 facing = enemy.MoveSpeed < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, facing, enemy.DetectionDistance, playerMask);
        Debug.DrawRay(rayOrigin, (Vector3)(facing * enemy.DetectionDistance), Color.blue);
        return hit.collider != null;
    }
}