using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    public Transform boxHolder;
    public Transform grabDetection;
    public float rayDist = 1;
    private TestMouvement mov;

    private LayerMask boxLayer;

    InputAction grabAction;

    void Start()
    {
        grabAction = InputSystem.actions.FindAction("Grab");
        mov = GetComponent<TestMouvement>();
        boxLayer = LayerMask.GetMask("Box");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetection.position, Vector2.right * (float)Math.Cos(transform.rotation.eulerAngles.y * Math.PI / 180), rayDist, boxLayer);

        if (grabCheck && !mov.isCrouched)
        {
            if(grabAction.IsPressed())
            {
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                grabCheck.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                grabCheck.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
