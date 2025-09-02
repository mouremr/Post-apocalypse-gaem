using UnityEngine;
public class LadderInteractible : Interactible
{
    [SerializeField] private float topOffset = 0.2f;
    [SerializeField] private float bottomOffset = 0.2f;

    public Vector2 TopPosition => (Vector2)transform.position + new Vector2(0, GetComponent<Collider2D>().bounds.extents.y - topOffset);
    public Vector2 BottomPosition => (Vector2)transform.position - new Vector2(0, GetComponent<Collider2D>().bounds.extents.y - bottomOffset);

    public override void Interact(GameObject player)
    {
        Debug.Log("Test");
        StateMachine stateMachine = player.GetComponent<StateMachine>();
        if (stateMachine != null && stateMachine.CurrentState is not ClimbingState)
        {
            stateMachine.ChangeState(new ClimbingState(stateMachine, this));
        }
    }

    // Visualize top and bottom positions
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(TopPosition, 0.1f);
        Gizmos.DrawWireSphere(BottomPosition, 0.1f);
    }
}