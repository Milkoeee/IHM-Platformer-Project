using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    [SerializeField] private AudioSource doorAS;
    [SerializeField] private AudioClip doorOpening;
    [SerializeField] private AudioClip doorClosing;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();


    }

    public void openDoor()
    {
        doorAS.clip = doorOpening;
        doorAS.Play();
        sr.enabled = false;
        col.enabled = false;   
    }

    public void closeDoor()
    {
        doorAS.clip = doorClosing;
        doorAS.Play();
        sr.enabled = true;
        col.enabled = true;
    }
}
