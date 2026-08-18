using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InteractionDetector))]
public class InteractionHandler : MonoBehaviour
{
    private PlayerInput input;
    private InteractionDetector detector;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        detector = GetComponent<InteractionDetector>();
    }
}
