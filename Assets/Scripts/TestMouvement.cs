using UnityEngine;

public class TestMouvement : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    InputAction moveAction;
    InputAction AButton;
    InputAction BButton;
    InputAction XButton;
    InputAction YButton;
    InputAction StartButton;

    float quitTime = 1;
    float totalTimePressed = 0;

    SpriteRenderer sprite;



    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        AButton = InputSystem.actions.FindAction("Jump");
        BButton = InputSystem.actions.FindAction("Crouch");
        XButton = InputSystem.actions.FindAction("Attack");
        YButton = InputSystem.actions.FindAction("Interact");
        StartButton = InputSystem.actions.FindAction("Start");

        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 4. Read the "Move" action value, which is a 2D vector
        // and the "Jump" action state, which is a boolean value

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        // your movement code here

        transform.position = (Vector3)moveValue;

        if (AButton.IsPressed())
        {
            sprite.color = Color.green;
        }
        if (AButton.WasReleasedThisFrame())
        {
            sprite.color = Color.white;
        }

        if (BButton.IsPressed())
        {
            sprite.color = Color.red;
        }
        if (BButton.WasReleasedThisFrame())
        {
            sprite.color = Color.white;
        }

        if (XButton.IsPressed())
        {
            sprite.color = Color.blue;
        }
        if (XButton.WasReleasedThisFrame())
        {
            sprite.color = Color.white;
        }

        if (YButton.IsPressed())
        {
            sprite.color = Color.yellow;
        }
        if (YButton.WasReleasedThisFrame())
        {
            sprite.color = Color.white;
        }

        if (StartButton.IsPressed())
        {
            totalTimePressed += Time.deltaTime;
            if (totalTimePressed >= quitTime)
            {
                Application.Quit();
            }
        }
        if (YButton.WasReleasedThisFrame())
        {
            totalTimePressed = 0;
        }


    }
}
