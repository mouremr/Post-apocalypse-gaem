//using System;
//using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    private Camera Cam;

    private Bounds camerabounds;

    private Vector3 targetPos;

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
        Cam = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        //float ppu = 16f; // match your sprite's Pixels Per Unit setting
        float height = Cam.orthographicSize;
        float width = height * Cam.aspect;
        
        float minX = Mathf.Min(Globals.WorldBounds.min.x + width, Globals.WorldBounds.max.x - width);
        float maxX = Mathf.Max(Globals.WorldBounds.min.x + width, Globals.WorldBounds.max.x - width);

        camerabounds = new Bounds();
        camerabounds.SetMinMax(
            new Vector3(minX, 0, 0),
            new Vector3(maxX, 0, 0)
);
        //Debug.Log($"Camera bounds min: {camerabounds.min}, max: {camerabounds.max}");

    }

    void LateUpdate()  // LateUpdate reduces jitter
    {
        if (playerTarget == null) return;

        targetPos = playerTarget.position + offset;

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

        targetPos = Vector3.SmoothDamp(
            transform.position,   // current
            targetPos,            // target
            ref velocity,         // current velocity ref
            smoothTime            // how long until close to target
        );

        targetPos = GetCameraBounds();


        transform.position = targetPos;
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(targetPos.x, camerabounds.min.x, camerabounds.max.x),
            targetPos.y,
            transform.position.z
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
