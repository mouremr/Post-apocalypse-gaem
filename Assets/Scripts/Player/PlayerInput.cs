using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // todo: Convert this to scriptable obj?

    // void Awake()
    // {
    //     DontDestroyOnLoad(this.gameObject);
    // }
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool RollPressed { get; private set; }
    public bool LightAttackPressed {get; private set;}
    public bool HeavyAttackPressed {get; private set;}
    public bool ToggleInventory {get; private set;}
    public bool PlayerControlsEnabled {get; set;}

    private void Awake()
    {
        PlayerControlsEnabled = true;
    }
    private void Update()
    {
        if(PlayerControlsEnabled)
        {
            InteractPressed = Input.GetKeyDown(KeyCode.E);
            JumpPressed = Input.GetButtonDown("Jump");
            JumpReleased = Input.GetButtonUp("Jump");
            RollPressed = Input.GetKeyDown(KeyCode.LeftShift);
            LightAttackPressed = Input.GetMouseButtonDown(0);
            HeavyAttackPressed = Input.GetMouseButtonDown(1);
            HorizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            InteractPressed = false;
            JumpPressed = false;
            JumpReleased = false;
            RollPressed = false;
            LightAttackPressed = false;
            HeavyAttackPressed = false;
            HorizontalInput = 0f;
        }
        
        ToggleInventory = Input.GetKeyDown(KeyCode.Tab);


        
    }
    public void ConsumeRoll()
    {
        RollPressed = false;
    }
}