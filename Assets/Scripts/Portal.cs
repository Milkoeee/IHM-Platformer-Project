using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private HashSet<GameObject> portalObjects = new HashSet<GameObject>();
    [SerializeField] private Portal destination;
    BoxCollider2D portalCollider;
    AudioSource portalSound;
    Vector2 portalSize;

    void Start()
    {
        portalCollider = GetComponent<BoxCollider2D>();
        portalSound = GetComponent<AudioSource>();
        portalSize = portalCollider.size;
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int sideCollision = 1;
        float yOffset = 0;

        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        if (collision.transform.position.x - transform.position.x < 0)
        {
            sideCollision = -1;
        }

        yOffset = collision.transform.position.y - transform.position.y;

        collision.transform.position = new Vector3(destination.transform.position.x + sideCollision * destination.portalSize.x / 2, destination.transform.position.y + yOffset, 0);

        portalSound.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalObjects.Remove(collision.gameObject);
    }
}
