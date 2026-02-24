using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Collider2D playerCollider;

    private float disableTime = 1f; // Time to ignore collision

    // Update is called once per frame
void Update()
    {
        // Check for down key press (S, Down Arrow, or controller down)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DisableCollisionTemporarily());
        }
    }
    
    System.Collections.IEnumerator DisableCollisionTemporarily()
    {
        // Get the player's collider
        // Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player")
        //     .GetComponent<Collider2D>();
        

        // Ignore collision between player and platform
        Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            
        // Wait for specified time
        yield return new WaitForSeconds(disableTime);
            
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }
}
