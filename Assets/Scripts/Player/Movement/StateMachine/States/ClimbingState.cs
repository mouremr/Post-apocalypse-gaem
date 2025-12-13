using UnityEngine;

public class ClimbingState : PlayerState
{
    private float climbSpeed = 3f;
    private LadderInteractible currentLadder;
    private InteractionDetector interactionDetector;

    public ClimbingState(StateMachine stateMachine, LadderInteractible ladder) : base(stateMachine) 
    {
        interactionDetector = stateMachine.InteractionDetector;
        currentLadder = ladder;
    }

    public override void Enter()
    {
        rb.gravityScale = 0;
        animator.SetBool("climbing", true);
        
        // Snap to ladder center
        Vector2 newPosition = new Vector2(
            currentLadder.transform.position.x,
            Mathf.Clamp(
                player.transform.position.y, 
                currentLadder.BottomPosition.y, 
                currentLadder.TopPosition.y
            )
        );
        player.transform.position = newPosition;
    }

    public override void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            stateMachine.ChangeState(new JumpingState(stateMachine, 7f, player.transform.position.y, 15f)); // Or appropriate state
            return;
        }
        
        // Exit climbing state when pressing E again or other conditions
        if (input.InteractPressed || currentLadder == null || ShouldExitClimbing())
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }

        // Handle horizontal movement to detach from ladder
        if (input.HorizontalInput != 0 && Mathf.Abs(input.HorizontalInput) > 0.1f)
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
        }

        if (rb.linearVelocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }

    }

    public override void FixedUpdate()
    {
        // Vertical climbing movement
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) verticalInput = -1f;
        
        rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);
        
        // Constrain player to ladder bounds
        if (currentLadder != null)
        {
            float clampedY = Mathf.Clamp(
                player.transform.position.y, 
                currentLadder.BottomPosition.y, 
                currentLadder.TopPosition.y
            );
            
            player.transform.position = new Vector2(
                currentLadder.transform.position.x,
                clampedY
            );
        }
    }

    public override void Exit()
    {
        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        //animator.SetBool("IsClimbing", false);
    }

    private bool ShouldExitClimbing()
    {
        // Exit if at bottom of ladder and pressing down
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && 
            player.transform.position.y <= currentLadder.BottomPosition.y)
        {
            return true;
        }
        
        return false;
    }
}
