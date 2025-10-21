using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void openDoor()
    {
        sr.enabled = false;
        col.enabled = false;   
    }

    public void closeDoor()
    {
        sr.enabled = true;
        col.enabled = true;
    }
}
