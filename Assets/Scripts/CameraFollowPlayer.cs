using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
    public float lookDownValue;
    public float lookUpValue;

    public float holdTime;

    private float currentHoldTime;

    InputAction lookDownAction;
    InputAction lookUpAction;

    public Vector2 cameraSpeed;
    public float smoothDuration;
    Vector2 cameraPosition;

    void Start()
    {
        lookDownAction = InputSystem.actions.FindAction("LookDown");
        lookUpAction = InputSystem.actions.FindAction("LookUp");
    }

    void Update()
    {
        PlayerLook();
        CameraFollowPlayerRotation();
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -1); // Camera follows the player with specified offset position
    }

    void CameraFollowPlayerRotation()
    {
        if (player.rotation.eulerAngles.y == 0)
        {
            cameraPosition = Vector2.SmoothDamp(cameraPosition, new Vector2(player.position.x + offset.x, player.position.y + offset.y), ref cameraSpeed, smoothDuration);
        }
        else
        {
            cameraPosition = Vector2.SmoothDamp(cameraPosition, new Vector2(player.position.x - offset.x, player.position.y + offset.y), ref cameraSpeed, smoothDuration);
        }
    }

    void LookDown()
    {
        offset = new Vector2(6, lookDownValue);
    }

    void LookUp()
    {
        offset = new Vector2(6, lookUpValue);
    }

    void ResetLook()
    {
        offset = new Vector2(6, 2);
    }

    void PlayerLook()
    {
        if (lookDownAction.IsPressed() && currentHoldTime > holdTime)
        {
            LookDown();
        }
        if (lookUpAction.IsPressed() && currentHoldTime > holdTime)
        {
            LookUp();
        }

        if (lookDownAction.IsPressed() || lookUpAction.IsPressed())
        {
            currentHoldTime += Time.deltaTime;
        }
        else
        {
            ResetLook();
            currentHoldTime = 0;
        }

    }
}
