using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool RollPressed { get; private set; }
    public bool AttackPressed {get; private set;}


    private void Update()
    {
        InteractPressed = Input.GetKeyDown(KeyCode.E);
        JumpPressed = Input.GetButtonDown("Jump");
        JumpReleased = Input.GetButtonUp("Jump");
        RollPressed = Input.GetKeyDown(KeyCode.LeftShift);
        AttackPressed = Input.GetMouseButtonDown(0);

        HorizontalInput = Input.GetAxis("Horizontal");
    }
    public void ConsumeRoll()
    {
        RollPressed = false;
    }
}