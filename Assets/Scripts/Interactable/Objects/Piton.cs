using UnityEngine;

public class Piton : Interactible
{
    private StateMachine playerStateMachine;
    public override void Interact()
    {
        if (playerStateMachine.CurrentState is WallClimbingState)
        {
            playerStateMachine.ChangeState(playerStateMachine.PlayerStates.Hanging());
        }
        else if (playerStateMachine.CurrentState is PlayerHangingState)
        {
            
            playerStateMachine.ChangeState(playerStateMachine.PlayerStates.WallClimbing());
        }

    }

    void Start()
    {
        GameObject iris = GameObject.Find("Iris");
        if (iris != null)
        {
            playerStateMachine = iris.GetComponent<StateMachine>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
