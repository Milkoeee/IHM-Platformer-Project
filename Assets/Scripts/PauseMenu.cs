using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public bool isPaused = false;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isPaused) Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Home()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
        
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
