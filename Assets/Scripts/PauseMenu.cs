using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject cursor;
    [SerializeField] private InputActionAsset actions;

    public bool isPaused = false;

    private InputAction pauseAction;

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
        Time.timeScale = 1.0f;

        if (pauseAction != null) pauseAction.performed += GamepadPause;

    }

    private void OnDestroy()
    {
        if (pauseAction != null) pauseAction.performed -= GamepadPause;
    }

    private void GamepadPause(InputAction.CallbackContext context)
    {
        if (!isPaused) Pause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        cursor.SetActive(true);

        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        cursor.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync(0);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
        Time.timeScale = 1f;
    }
}

