using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movement;
    [SerializeField] ParticleSystem fall;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speedThreshold = 2f;   
    [SerializeField] float movementDelay = 0.3f;

    TestMouvement playerMovement;
    float moveTimer;
    bool wasGrounded;

    private void Awake()
    {
        playerMovement = GetComponentInParent<TestMouvement>();
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;

        if (!playerMovement.inAir && Mathf.Abs(rb.linearVelocityX) > speedThreshold)
        {
            if (moveTimer >= movementDelay)
            {
                movement.Play();
                moveTimer = 0f;
            }
        }

        if (!playerMovement.inAir && !wasGrounded)
        {
            wasGrounded = true;
            if (fall != null && !fall.isPlaying)
                fall.Play();
        }
        else if (playerMovement.inAir)
        {
            wasGrounded = false;
        }
    }
}