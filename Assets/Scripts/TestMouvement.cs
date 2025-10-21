using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMouvement : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    private SpriteRenderer sp;

    InputAction moveAction;
    InputAction AButton;
    InputAction sprintAction;
    InputAction crouchAction;

    public Rigidbody2D rb;
    public BoxCollider2D bc2d;

    public float jumpSpeed = 20;
    public float sprintFactor = 2.0f;
    public float crouchFactor = 0.5f;

    public float speedModifier = 5;
    public float currentSpeed;

    bool maxJump = false;
    bool isJumping = false;

    [SerializeField] float maxJumpTime = 1;
    float curJumpTime = 0;

    public bool isCrouched;
    public float crouchHold;
    public Vector3 originalScale;

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
    }

    void Update()
    {
        // 4. Read the "Move" action value, which is a 2D vector
        // and the "Jump" action state, which is a boolean value
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveValue.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveValue.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

        bool isSprinting = sprintAction.IsPressed() && !isCrouched;
        currentSpeed = isSprinting ? speedModifier * sprintFactor : isCrouched ? speedModifier * crouchFactor : speedModifier;

        if (crouchAction.IsPressed())
        {
            crouchHold += Time.deltaTime;
            if (crouchHold >= 0.8f && !isCrouched)
            {
                isCrouched = true;
                transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchFactor, originalScale.z);
                float difference = (transform.localScale.y - originalScale.y) / 2f;
                transform.position += Vector3.up * difference;
            }
        }
        else
        {
            crouchHold = 0f;
        }

        if (isCrouched && !crouchAction.IsPressed())
        {
            isCrouched = false;
            float difference = (originalScale.y - transform.localScale.y) / 2f;
            transform.localScale = originalScale; 
            transform.position += Vector3.up * difference;
        }

        rb.linearVelocityX = currentSpeed * moveValue.x;

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
