using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{

    bool finished = false;
    private AudioSource finishSound;
    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (finished && !finishSound.isPlaying)
        {
            Time.timeScale = 0.0f;
            SceneManager.LoadScene("Scenes/Main Menu", LoadSceneMode.Single);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player") && !finished)
        {
            UnlockNextLevel();
            finishSound.Play();
            finished = true;
        } 
    }

    void UnlockNextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index >= PlayerPrefs.GetInt("Unlocked"))
        {
            PlayerPrefs.SetInt("Unlocked", index + 1);
            PlayerPrefs.Save();
        }
    }
}
