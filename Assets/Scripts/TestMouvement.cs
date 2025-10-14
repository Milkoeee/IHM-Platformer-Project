using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using System;




public class TestMouvement : MonoBehaviour
{
    InputAction moveAction;
    InputAction AButton;
    InputAction sprintAction;
    public Rigidbody2D rb;
    public float jumpSpeed = 20;
    public float sprintFactor = 2.0f;

    public float speedMofifier = 5;
    public float maxJumpTime = 1;

    bool maxJump = false;
    bool isJumping = false;
    public float bufferTime = 0.1f;
    bool canBeBuffered = false;
    bool processBufferAction = false;
    float curJumpTime = 0;
    bool pressing = false;

    float totalSpeed = 0;

    public int raycastLength = 30;

    bool blockingLeft = false;
    bool blockingRight = false;

    public int preFrames = 1;
    public int nRays = 1;

    LayerMask layerMask;

    Queue<float> bufferQueue = new Queue<float>();
    BoxCollider2D collider;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Wall");
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        bool isSprinting = sprintAction.IsPressed();
        float currentSpeed = isSprinting ? speedMofifier * sprintFactor : speedMofifier;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        totalSpeed = currentSpeed * moveValue.x;


        CollisionRayCast();
        if (blockingLeft)
        {
            if (moveValue.x < 0)
            {
                totalSpeed = 0;
            }
        }
        if (blockingRight)
        {
            if (moveValue.x > 0)
            {
                totalSpeed = 0;
            }
        }

        rb.linearVelocityX = totalSpeed;


        ProcessJump();
        ProcessBufferQueue();

    }

    void JumpInit()
    {
        isJumping = true;
        canBeBuffered = false;
        processBufferAction = false;
    }

    void Jump()
    {
        pressing = true;
        rb.linearVelocityY = jumpSpeed;
        if (curJumpTime >= maxJumpTime) maxJump = true;
        curJumpTime += Time.deltaTime;
    }

    void ProcessJump()
    {
        if (AButton.IsPressed() && Mathf.Abs(rb.linearVelocityY) <1E-06f && !isJumping && !pressing)
        {
            JumpInit();
        }

        if (AButton.IsPressed() && canBeBuffered)
        {
            pressing = true;
            bufferQueue.Enqueue(Time.time);
        }

        if (AButton.IsPressed() && !maxJump && isJumping)
        {
            Jump();
        }


        if (Mathf.Abs(rb.linearVelocityY) <1E-06f && !isJumping)
        {
            maxJump = false;
            curJumpTime = 0;
            processBufferAction = true;
        }

        if (AButton.WasReleasedThisFrame())
        {
            isJumping = false;
            canBeBuffered = true;
            pressing = false;
        }
    }

    void ProcessBufferQueue()
    {
        foreach (float inputTime in bufferQueue)
        {
            if (Time.time > inputTime + bufferTime)
            {
                bufferQueue.Dequeue();
                break;
            }
            else if (processBufferAction)
            {
                JumpReset();
                ProcessJump();
                bufferQueue.Clear();
                break;
            }
        }
    }

    void JumpReset()
    {
        maxJump = false;
        curJumpTime = 0;
        processBufferAction = true;
        isJumping = false;
        pressing = false;
    }

    void CollisionRayCast()
    {
        blockingLeft = false;
        blockingRight = false;
        for (int i=0; i < nRays; i++)
        {
            if (Physics2D.Raycast(transform.position - new Vector3(collider.size.x / 2, (float) Math.Pow(-1, i)*(i+1)*collider.size.y/nRays, 0), transform.TransformDirection(Vector3.left), Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, layerMask))
            {
                Debug.DrawRay(transform.position - new Vector3(collider.size.x / 2, (float)Math.Pow(-1, i) * (i+1) * collider.size.y / nRays, 0), transform.TransformDirection(Vector3.left) * Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, Color.yellow);
                Debug.Log("Did Hit Left");
                blockingLeft = true;
            }
            
            if (Physics2D.Raycast(transform.position + new Vector3(collider.size.x / 2, (float) Math.Pow(-1, i)*(i+1)*collider.size.y/nRays, 0), transform.TransformDirection(Vector3.right), Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, layerMask))
            {
                Debug.DrawRay(transform.position + new Vector3(collider.size.x / 2, (float)Math.Pow(-1, i) * (i+1) * collider.size.y / nRays, 0), transform.TransformDirection(Vector3.right) * Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, Color.yellow);
                Debug.Log("Did Hit Right");
                blockingRight = true;
            }   
        }
    }
}
