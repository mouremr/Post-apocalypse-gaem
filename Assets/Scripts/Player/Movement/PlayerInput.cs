using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
        InteractPressed = Input.GetKeyDown(KeyCode.E);
    }
}