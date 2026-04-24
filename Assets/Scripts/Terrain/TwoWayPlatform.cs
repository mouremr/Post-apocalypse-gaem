using UnityEngine;

public class TwoWayPlatform : MonoBehaviour
{
    //[SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private GameObject platform;


    private bool isOnPlatform = false;
    private bool isFallingThrough = false;
    [SerializeField] private LayerMask playerMask;
    private Vector2 boxCenter;
    private Vector2 boxSize = Vector2.zero;

    private void Start()
    {
        boxCenter = platformCollider.bounds.center;
        boxSize = platformCollider.bounds.size;
    }



    void Update()
    {
        if (isOnPlatform && Input.GetKeyDown(KeyCode.S) && !isFallingThrough)
        {
            
            isFallingThrough = true;
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            platform.tag = "Untagged";
        }



        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.up,
            .5f,
            playerMask
        );


        if (hit.collider == null && isFallingThrough)
        {
            isFallingThrough = false;
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
            platform.tag = "Mantleable";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == playerCollider)
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == playerCollider)
        {
            isOnPlatform = false;

        }
    }
}
