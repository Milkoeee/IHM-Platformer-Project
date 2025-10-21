using UnityEngine;

public class ButtonDoorOpening : MonoBehaviour
{
    public Door _door;
    private float timer;

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) _door.closeDoor();
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Box"))
        {

            _door.openDoor();
            timer = 0.5f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Box")) timer = 0.5f;
    }

}
