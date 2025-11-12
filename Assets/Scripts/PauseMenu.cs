using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject cursor;
    [SerializeField] private InputActionAsset actions;

    public bool isPaused = false;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    private void Awake()
    {
        playerMap = actions.FindActionMap("Player", true);
        uiMap = actions.FindActionMap("UI", true);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isPaused) Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        cursor.SetActive(true);
        playerMap.Disable();
        uiMap.Enable();

        isPaused = true;
        Time.timeScale = 0.0f;

    }

    public void Home()
    {
        playerMap.Enable();
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
    }

    public void Resume()
    {
        cursor.SetActive(false);
        playerMap.Enable();
        uiMap.Disable();
        
        isPaused = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        cursor.SetActive(false);
        playerMap.Enable();
        uiMap.Disable();

        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
