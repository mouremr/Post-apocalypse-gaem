using UnityEngine;

public class TwoWayPlatform : MonoBehaviour
{
    //[SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Collider2D playerCollider;


    private bool isOnPlatform = false;
    private bool isFallingThrough = false;

    void Update()
    {
        if (isOnPlatform && Input.GetKeyDown(KeyCode.S) && !isFallingThrough)
        {
            isFallingThrough = true;
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
        //Debug.Log(isOnPlatform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == playerCollider)
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
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
            //Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
            if (isFallingThrough)
            {
                // Add a small delay to ensure player is fully through
                Invoke(nameof(ReenableCollision), .5f);
            }
        }
    }
    private void ReenableCollision()
    {
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        isFallingThrough = false;
    }
}
