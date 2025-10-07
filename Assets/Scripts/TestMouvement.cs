using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMouvement : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    InputAction moveAction;
    InputAction AButton;
    public Rigidbody2D rb;
    public float jumpSpeed = 20;

    public float speedMofifier = 5;

    bool maxJump = false;
    bool isJumping = false;

    [SerializeField] float maxJumpTime = 1;
    float curJumpTime = 0;

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 4. Read the "Move" action value, which is a 2D vector
        // and the "Jump" action state, which is a boolean value

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        // your movement code here

        rb.linearVelocityX = speedMofifier * moveValue.x;

        if (AButton.IsPressed() && rb.linearVelocityY == 0 && !isJumping) isJumping = true;

        if (AButton.IsPressed() && !maxJump && isJumping)
        {
            rb.linearVelocityY = jumpSpeed;
            if (curJumpTime >= maxJumpTime) maxJump = true;
            curJumpTime += Time.deltaTime;
        }


        if (rb.linearVelocityY == 0 && !isJumping)
        {
            maxJump = false;
            curJumpTime = 0;
        }

        if (AButton.WasReleasedThisFrame())
        {
            isJumping = false;
        }

    }
}
