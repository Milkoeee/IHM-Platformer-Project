using UnityEngine;
using UnityEngine.Rendering;


public class PlayerAudioController : MonoBehaviour
{
    public enum soundID
    {
        JUMP,
        BOOST,
        SLOW,
        GO
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public AudioClip jumpSound;
    public AudioClip boostSound;
    public AudioClip slowSound;
    public AudioClip gameOverSound;
    public AudioClip bgMusic;

    [SerializeField] public AudioSource sourceMusic;
    [SerializeField] public AudioSource sourceSFX;

    void Start()
    {
        sourceMusic.clip = bgMusic;
        sourceMusic.Play();
    }

    public void PlaySound(soundID id)
    {
        switch (id)
        {
            case soundID.JUMP:
                sourceSFX.clip = jumpSound;
                break;
            case soundID.BOOST:
                sourceSFX.clip = boostSound;
                break;
            case soundID.SLOW:
                sourceSFX.clip = slowSound;
                break;
            case soundID.GO:
                sourceSFX.clip = gameOverSound;
                break;
            default:
                return;
        }

        sourceSFX.Play();
    }
}
