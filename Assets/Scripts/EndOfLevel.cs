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
            finishSound.Play();
            //SceneManager.LoadScene("Scenes/Main Menu", LoadSceneMode.Single);
        } 
    }
}
