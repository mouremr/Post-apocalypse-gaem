using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f; // higher = snappier
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private bool snapNextFrame = true;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            if (snapNextFrame)
            {
                transform.position = targetPosition; // instant on first frame
                snapNextFrame = false;
            }
            else
            {
                // Interpolate towards target each FixedUpdate
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            snapNextFrame = true;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
