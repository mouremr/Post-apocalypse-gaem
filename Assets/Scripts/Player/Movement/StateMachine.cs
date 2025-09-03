
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private PlayerState _currentState;
    private InteractionDetector interactionDetector;

    public PlayerState CurrentState => _currentState;
    public InteractionDetector InteractionDetector => interactionDetector;

    private void Start()
    {
        interactionDetector = GetComponent<InteractionDetector>();
        ChangeState(new GroundedState(this));
    }

    private void Update()
    {
        _currentState?.Update();
        // Handle global interaction input
        if (Input.GetKeyDown(KeyCode.E) && interactionDetector.HasInteractible)
        {
            interactionDetector.CurrentInteractible.Interact(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"State: {_currentState?.GetType().Name}");
    }
}