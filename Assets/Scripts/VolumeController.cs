using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    public void Start()
    {
        if(PlayerPrefs.HasKey("music")) LoadMusicVolume();
        else SetMusicVolume();
        if (PlayerPrefs.HasKey("music")) LoadSfxVolume();
        else SetSfxVolume();
    }
    public void SetMusicVolume()
    {
        if (audioMixer != null)
        {
            float musicVolume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(musicVolume)*20);
            PlayerPrefs.SetFloat("music", musicVolume);
        }
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("music");
        SetMusicVolume();
    }

    public void SetSfxVolume()
    {
        if (audioMixer != null)
        {
            float sfxVolume = sfxSlider.value;
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume)*20);
            PlayerPrefs.SetFloat("sfx", sfxVolume);
        }
    }

    private void LoadSfxVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfx");
        SetSfxVolume();
    }
}
