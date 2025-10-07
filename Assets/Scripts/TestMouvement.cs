using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;




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

    Queue<float> bufferQueue = new Queue<float>();

    private void Start()
    {

        Debug.Log("called " + System.Reflection.MethodBase.GetCurrentMethod().Name + " at " + Time.time + "s");
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        bool isSprinting = sprintAction.IsPressed();
        float currentSpeed = isSprinting ? speedMofifier * sprintFactor : speedMofifier;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        rb.linearVelocityX = currentSpeed * moveValue.x;

        ProcessJump();
        ProcessBufferQueue();

    }

    void JumpInit()
    {
        Debug.Log("called " + System.Reflection.MethodBase.GetCurrentMethod().Name + " at " + Time.time + "s");
        isJumping = true;
        canBeBuffered = false;
        processBufferAction = false;

    }

    void Jump()
    {
        Debug.Log("called " + System.Reflection.MethodBase.GetCurrentMethod().Name + " at " + Time.time + "s");
        rb.linearVelocityY = jumpSpeed;
        if (curJumpTime >= maxJumpTime) maxJump = true;
        curJumpTime += Time.deltaTime;
    }

    void ProcessJump()
    {
        if (AButton.IsPressed() && rb.linearVelocityY == 0 && !isJumping)
        {
            JumpInit();
        }

        if (AButton.IsPressed() && canBeBuffered)
        {
            Debug.Log("Buffered A");
            bufferQueue.Enqueue(Time.time);
        }

        if (AButton.IsPressed() && !maxJump && isJumping)
        {
            Jump();
        }


        if (rb.linearVelocityY == 0 && !isJumping)
        {
            Debug.Log("Touched ground");
            maxJump = false;
            curJumpTime = 0;
            processBufferAction = true;
        }

        if (AButton.WasReleasedThisFrame())
        {
            Debug.Log("Released A");
            isJumping = false;
            canBeBuffered = true;
        }
    }

    void ProcessBufferQueue()
    {
        foreach (float inputTime in bufferQueue)
        {
            if (Time.time > inputTime + bufferTime)
            {
                Debug.Log((inputTime + bufferTime) + "   " + Time.time + "   " + "Dequeue");
                bufferQueue.Dequeue();
                break;
            }
            else if (processBufferAction)
            {
                Debug.Log((inputTime + bufferTime) + "   " + Time.time + "   " + "Buffered Jump");
                JumpReset();
                ProcessJump();
                bufferQueue.Clear();
                break;
            }
        }
    }

    void JumpReset()
    {
        Debug.Log("called " + System.Reflection.MethodBase.GetCurrentMethod().Name + " at " + Time.time + "s");
        maxJump = false;
        curJumpTime = 0;
        processBufferAction = true;
        isJumping = false;
    }
}
