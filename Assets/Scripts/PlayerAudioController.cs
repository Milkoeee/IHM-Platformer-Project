using UnityEngine;
using UnityEngine.Rendering;


public class PlayerAudioController : MonoBehaviour
{
    public enum soundID
    {
        JUMP,
        BOOST,
        SLOW
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public AudioClip jumpSound;
    public AudioClip boostSound;
    public AudioClip slowSound;

    public AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(soundID id)
    {
        switch (id)
        {
            case soundID.JUMP:
                source.clip = jumpSound;
                break;
            case soundID.BOOST:
                source.clip = boostSound;
                break;
            case soundID.SLOW:
                source.clip = slowSound;
                break;
            default:
                return;
        }

        source.Play();
    }
}
