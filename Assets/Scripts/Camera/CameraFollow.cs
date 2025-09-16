using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    [Header("Follow Settings")]
    [SerializeField] public float smoothTime = 0.15f;  // time for camera to reach target
    [SerializeField] private float deadZone = 0.01f;    // ignore micro jitter

    private Vector3 velocity = Vector3.zero;
    private bool snapNextFrame = true;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void LateUpdate()  // LateUpdate reduces jitter
    {
        if (playerTarget == null) return;

        Vector3 targetPos = playerTarget.position + offset;

        if (snapNextFrame)
        {
            transform.position = targetPos;  // snap instantly once
            snapNextFrame = false;
            return;
        }

        Vector3 delta = targetPos - transform.position;

        if (delta.magnitude < deadZone)
            return;

        // SmoothDamp with damping
        transform.position = Vector3.SmoothDamp(
            transform.position,   // current
            targetPos,            // target
            ref velocity,         // current velocity ref
            smoothTime            // how long until close to target
        );
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;

            Transform headTarget = player.transform.Find("CameraHeadTarget");
            if (headTarget != null) playerTarget = headTarget;

            snapNextFrame = true;
            velocity = Vector3.zero;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
