using UnityEngine;

public class TwoWayPlatform : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Collider2D playerCollider;

    private float disableTime = 1f; // time to ignore collision

void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //disable collision for some amount of time
            StartCoroutine(DisableCollisionTemporarily());
        }
    }
    
    System.Collections.IEnumerator DisableCollisionTemporarily()
    {
        

        // ignore collision between player and platform and wait
        Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            

        yield return new WaitForSeconds(disableTime);
            
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }
}
