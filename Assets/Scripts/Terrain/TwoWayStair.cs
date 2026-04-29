using UnityEngine;

public class TwoWayStair : MonoBehaviour
{
    //[SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Collider2D stairsCollider;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private GameObject stairs;


    private bool isOnPlatform = false;
    private bool isFallingThrough = false;
    [SerializeField] private LayerMask playerMask;
    private Vector2 boxCenter;
    private Vector2 boxSize = Vector2.zero;

    private RaycastHit2D bottomHit;
    private RaycastHit2D topHit;
    Vector2 bottomCastSize;
    Vector2 bottomCastOrigin;
    Vector2 boundsMin;
    Vector2 boundsMax;

    private void Start()
    {
        boundsMin = stairsCollider.bounds.min;
        boundsMax = stairsCollider.bounds.max;
        boxCenter = stairsCollider.bounds.center;
        boxSize = stairsCollider.bounds.size;
        bottomCastSize = new Vector2(boxSize.x, 0.5f);
        bottomCastOrigin = new Vector2(boxCenter.x, stairsCollider.bounds.min.y - 0.2f);
    }



    void Update()
    {
        //Debug.Log(isOnPlatform);
        bottomHit = Physics2D.Linecast(
            boundsMin - new Vector2(0,0.1f),
            boundsMax - new Vector2(0,.6f), //subtract to move ray to bottom corner of top peice
            playerMask
        );
        topHit = Physics2D.Linecast(
            boundsMin + new Vector2(0,.7f),
            boundsMax + new Vector2(0,.1f),
            playerMask
        );

        //if (topHit.collider != null) Debug.Log("Top hit: " + topHit.collider.tag);
        //if (bottomHit.collider != null) Debug.Log("Bottom hit: " + bottomHit.collider.tag);

        if (isOnPlatform && Input.GetKeyDown(KeyCode.S) && !isFallingThrough)
        {
            
            isFallingThrough = true;
            Physics2D.IgnoreCollision(stairsCollider, playerCollider, true);
            stairs.tag = "Untagged";
        }

        // Bottom detection - player is below/behind the stair
        if (bottomHit.collider != null)
        {
            Physics2D.IgnoreCollision(stairsCollider, playerCollider, true);
            stairs.tag = "Untagged";
        }
        else if (!isFallingThrough)
        {
            Physics2D.IgnoreCollision(stairsCollider, playerCollider, false);
            stairs.tag = "Mantleable";
        }


        //Cast above the platform to check for player standing above platform
    

        if (topHit.collider == null && isFallingThrough)
        {
            isFallingThrough = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (stairsCollider == null) return;

        
        Gizmos.color = Color.red;

        Gizmos.DrawLine(stairsCollider.bounds.min - new Vector3(0,0.1f), stairsCollider.bounds.max - new Vector3(0,.6f));
        Gizmos.DrawLine(stairsCollider.bounds.min + new Vector3(0,.7f), stairsCollider.bounds.max + new Vector3(0,.1f));
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
