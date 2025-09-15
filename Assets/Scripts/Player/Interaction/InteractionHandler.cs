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

    private void Update()
    {
        // Detect if E is pressed and an interactible is nearby
        if (input.InteractPressed && detector.HasInteractible)
        {
            Debug.Log($"Interacting with: {detector.CurrentInteractible.name}");
            detector.CurrentInteractible.Interact(gameObject);
        }
    }
}
