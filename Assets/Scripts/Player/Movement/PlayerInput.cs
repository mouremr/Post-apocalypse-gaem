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
    public bool SlidePressed { get; private set; }


    private void Update()
    {
        InteractPressed = Input.GetKeyDown(KeyCode.E);
        JumpPressed = Input.GetButtonDown("Jump");
        JumpReleased = Input.GetButtonUp("Jump");
        SlidePressed = Input.GetKeyDown(KeyCode.LeftShift);

        HorizontalInput = Input.GetAxis("Horizontal");
    }
}