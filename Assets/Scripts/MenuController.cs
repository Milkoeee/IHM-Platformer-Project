using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlocked = PlayerPrefs.GetInt("Unlocked", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i < unlocked);
        }
    }

    private void Start()
    {
        foreach (var btn in buttons)
        {
            if (btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                break;
            }
        }
    }

    public void PlayLevel(int levelId)
    {
        SceneManager.LoadSceneAsync("Scenes/Niv" + levelId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
