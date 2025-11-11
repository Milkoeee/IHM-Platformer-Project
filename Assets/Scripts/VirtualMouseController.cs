using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseController : MonoBehaviour
{
    private VirtualMouseInput cursor;

    private void Awake()
    {
        cursor = GetComponent<VirtualMouseInput>();
    }
    private void LateUpdate()
    {
        Vector2 pos = cursor.virtualMouse.position.value;
        pos.x = Mathf.Clamp(pos.x, 0f, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0f, Screen.height);
        InputState.Change(cursor.virtualMouse.position, pos);
    }
}

