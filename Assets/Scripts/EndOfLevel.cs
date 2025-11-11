using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{

    private AudioSource finishSound;
        void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            UnlockNextLevel();
            finishSound.Play();
            //SceneManager.LoadScene("Scenes/Main Menu", LoadSceneMode.Single);
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
