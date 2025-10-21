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
    bool blockingLeft = false;
    bool blockingRight = false;
    bool isSprinting = false;
    public float currentSpeed;

    bool isSlowed = false;
    public float slowModifier = 2;

    bool isBoosted = false;
    public float boostModifier = 2;

    public int preFrames = 1;
    public int nRays = 1;

    LayerMask layerMask;

    Queue<float> bufferQueue = new Queue<float>();
    BoxCollider2D playerCollider;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Wall");
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    void Update()
    {
        bool isSprinting = sprintAction.IsPressed();
        if (Mathf.Abs(rb.linearVelocityY) < 1E-06f)
        {
            currentSpeed = isSprinting ? speedMofifier * sprintFactor : speedMofifier;
            currentSpeed = isSlowed ? currentSpeed / slowModifier : currentSpeed;
            currentSpeed = isBoosted ? boostModifier * currentSpeed : currentSpeed;
        }
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
        Vector2 colliderSize = playerCollider.size;
        blockingLeft = false;
        blockingRight = false;
        for (int i = 0; i < nRays; i++)
        {
            if (Physics2D.Raycast(transform.position - new Vector3(colliderSize.x / 2, 2 * i * colliderSize.y / (nRays - 1) - colliderSize.y, 0), transform.TransformDirection(Vector3.left), Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, layerMask))
            {
                blockingLeft = true;
            }

            if (Physics2D.Raycast(transform.position + new Vector3(colliderSize.x / 2, 2 * i * colliderSize.y / (nRays - 1) - colliderSize.y, 0), transform.TransformDirection(Vector3.right), Mathf.Abs(totalSpeed) * Time.deltaTime * preFrames, layerMask))
            {
                blockingRight = true;
            }

            if (Physics2D.Raycast(transform.position + new Vector3(i * colliderSize.x / (nRays - 1) - colliderSize.x / 2, colliderSize.y, 0), transform.TransformDirection(Vector3.up), Mathf.Abs(rb.linearVelocityY) * Time.deltaTime * preFrames, layerMask))
            {
                maxJump = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entering " + other.gameObject.name);
        if (other.gameObject.name.Equals("Slow")) isSlowed = true;
        if (other.gameObject.name.Equals("Boost")) isBoosted = true;    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Leaving " + other.gameObject.name);
        if (other.gameObject.name.Equals("Slow")) isSlowed = false;
        if (other.gameObject.name.Equals("Boost")) isBoosted = false;
    }
}
