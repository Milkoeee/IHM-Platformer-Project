using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject GOPanel;
    [SerializeField] float timeLeft = 5f;
    [SerializeField] float maxTime = 300;

    void Start()
    {
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;
            GameOver();
        }
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int sec = Mathf.FloorToInt(timeLeft%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, sec);
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;
        GOPanel.SetActive(true);
    }
    public void Home()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
        
    }
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);        
    }
}
