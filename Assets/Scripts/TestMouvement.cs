using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;



public class TestMouvement : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpButton;
    InputAction sprintAction;
    InputAction crouchAction;

    public PlayerAudioController audioController;

    public Rigidbody2D rb;
    public float jumpSpeed = 20;
    public float sprintFactor = 2.0f;
    public float crouchFactor = 0.5f;

    public float speedModifier = 5;
    public float maxJumpTime = 1;
    public float currentSpeed;

    bool maxJump = false;
    bool isJumping = false;
    public float bufferTime = 0.1f;
    public bool canBeBuffered = false;
    bool processBufferAction = false;
    float curJumpTime = 0;
    bool pressing = false;
    float totalSpeed = 0;
    bool isSprinting = false;

    public float inAirTime = 0;

    public int nRays = 10;
    public float raySize = 1;
    bool canUncrouch = true;

    BoxCollider2D playerCollider;

    LayerMask layers;

    bool multipleFloors = false;

    public int floorsCounter = 0;

    [SerializeField] bool inAir = false;
    [SerializeField] bool isSlowed = false;
    public float slowModifier = 2;

    [SerializeField] bool isBoosted = false;
    public float boostModifier = 2;

    Queue<float> bufferQueue = new Queue<float>();
    public bool isCrouched;    

    public Vector3 originalScale;

    private void Start()
    {
        layers = LayerMask.GetMask("Wall");
        playerCollider = GetComponent<BoxCollider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpButton = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (inAir) inAirTime += Time.deltaTime;
        else inAirTime = 0;
        isSprinting = sprintAction.IsPressed() && !isCrouched;
        if (!inAir)
        {
            currentSpeed = isSprinting ? speedModifier * sprintFactor : isCrouched ? speedModifier * crouchFactor : speedModifier;
            currentSpeed = isSlowed ? currentSpeed / slowModifier : currentSpeed;
            currentSpeed = isBoosted ? boostModifier * currentSpeed : currentSpeed;
        }
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveValue.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveValue.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

        ProcessCrouch();

        totalSpeed = currentSpeed * moveValue.x;

        rb.linearVelocityX = totalSpeed;


        ProcessJump();
        ProcessBufferQueue();

    }

    void ProcessCrouch()
    {
        if (crouchAction.IsPressed() && !isCrouched)
        {
            isCrouched = true;
            transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchFactor, originalScale.z);
            float difference = (transform.localScale.y - originalScale.y) / 2f;
            transform.position += Vector3.up * difference;
        }
        if (isCrouched && canUncrouch && !crouchAction.IsPressed())
        {
            isCrouched = false;
            float difference = (originalScale.y - transform.localScale.y) / 2f;
            transform.localScale = originalScale;
            transform.position += Vector3.up * difference;
        }

        if (isCrouched)
        {
            ProcessCrouchRays();
        }
    }

    void JumpInit()
    {
        audioController.PlaySound(PlayerAudioController.soundID.JUMP);
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
        if (inAirTime < bufferTime)
        {
            if (!isJumping) canBeBuffered = true;
        }
        else
        {
            canBeBuffered = false;
        }
        if (jumpButton.IsPressed() && (!inAir || inAirTime < bufferTime) && !isJumping && !pressing)
        {
            JumpInit();
        }

        if (jumpButton.IsPressed() && canBeBuffered)
        {
            pressing = true;
            bufferQueue.Enqueue(Time.time);
        }

        if (jumpButton.IsPressed() && !maxJump && isJumping)
        {
            Jump();
        }


        if (!inAir && !isJumping)
        {
            maxJump = false;
            curJumpTime = 0;
            processBufferAction = true;
        }

        if (jumpButton.WasReleasedThisFrame())
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
            Debug.Log(inputTime + "    " + inAirTime);
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Floor"))
        {
            floorsCounter--;
            if (!multipleFloors) inAir = true;
            if (floorsCounter <= 1) multipleFloors = false;
        } 
        if (other.gameObject.name.Equals("SlowSurface")) isSlowed = false;
        if (other.gameObject.name.Equals("BoostSurface")) isBoosted = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Ceiling")) maxJump = true;
        if (other.gameObject.name.Equals("Floor"))
        {
            floorsCounter++; 
            if (floorsCounter > 1)
            {
                multipleFloors = true;
            } 
            inAir = false;
        }
        if (other.gameObject.name.Equals("SlowSurface"))
        {
            isSlowed = true;
            audioController.PlaySound(PlayerAudioController.soundID.SLOW);

        }
        if (other.gameObject.name.Equals("BoostSurface"))
        {
            isBoosted = true;
            audioController.PlaySound(PlayerAudioController.soundID.BOOST);
        }
    }

    void ProcessCrouchRays()
    {
        int counter = 0;
        for (int i = 0; i < nRays; i++)
        {
            float offset = playerCollider.size.x;
            Vector2 src = new Vector2(transform.position.x - offset/2 + i*offset/nRays, transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(src, Vector2.up, raySize, layers);
            Debug.DrawRay(src, Vector2.up * raySize, Color.yellow);

            if (hit)
            {
                counter++;
            }
        }
        if (counter > 0)
        {
            canUncrouch = false;
        }
        else
        {
            canUncrouch = true;
        }
    }
}
