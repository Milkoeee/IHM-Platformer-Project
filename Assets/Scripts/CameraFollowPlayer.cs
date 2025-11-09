using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
    Vector2 cameraPosition;

    void Update()
    {
        CameraFollowPlayerRotation();
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -1); // Camera follows the player with specified offset position
    }
 
    void CameraFollowPlayerRotation()
    {
        if (player.rotation.eulerAngles.y == 0)
        {
            cameraPosition = new Vector2(player.position.x + offset.x, player.position.y + offset.y);
        }
        else
        {
            cameraPosition = new Vector2(player.position.x - offset.x, player.position.y + offset.y);
        }
    }
}
