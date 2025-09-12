using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          
    public float smoothing = 5f;      
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

private bool snapNextFrame = true;

void FixedUpdate()
{
    if (target != null)
    {
        Vector3 targetPosition = target.position + offset;

        if (snapNextFrame)
        {
            transform.position = targetPosition;
            snapNextFrame = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
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
