using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.UI;

public class VirtualMouseController : MonoBehaviour
{
    public RectTransform cursorTransform;
    public float cursorSpeed = 1000f;
    public Canvas canvas;

    private Mouse virtualMouse;
    private Vector2 currentPos;

    void OnEnable()
    {
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse);
    }

    void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);
    }

    void Update()
    {
        Vector2 move = Gamepad.current?.leftStick.ReadValue() ?? Vector2.zero;
        move *= cursorSpeed * Time.deltaTime;

        currentPos += move;

        // Constrain to screen
        currentPos.x = Mathf.Clamp(currentPos.x, 0, Screen.width);
        currentPos.y = Mathf.Clamp(currentPos.y, 0, Screen.height);

        // Move the virtual mouse
        InputState.Change(virtualMouse.position, currentPos);

        if (cursorTransform != null && canvas != null)
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                currentPos, canvas.worldCamera, out anchoredPos);
            cursorTransform.anchoredPosition = anchoredPos;
        }

        // Handle button press
        if (Gamepad.current != null)
        {
            bool pressed = Gamepad.current.aButton.isPressed;
            InputState.Change(virtualMouse.leftButton, pressed);
        }
    }
}

