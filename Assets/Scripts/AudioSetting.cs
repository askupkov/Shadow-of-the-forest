using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public static AudioSetting Instance { get; private set; }
    [SerializeField] GameObject settingPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private List<AudioSource> musicSource = new List<AudioSource>();
    private List<AudioSource> sfxSources = new List<AudioSource>();

    public bool settingOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        settingPanel.SetActive(false);
        // ”станавливаем значени€ слайдеров из PlayerPrefs или по умолчанию
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);

        // ѕримен€ем значени€ к аудио
        OnMusicSliderChanged();
        OnSfxSliderChanged();
    }

    public void openAudioSetting()
    {
        settingPanel.SetActive(true);
        settingOpen = true;
    }

    public void closeAudioSetting()
    {
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
        settingPanel.SetActive(false);
        settingOpen = false;
        if(GameInput.Instance != null)
        {
            GameInput.Instance.OnEnabled();
        }
        Time.timeScale = 1f;
        UnMute();
    }

    public void OnMusicSliderChanged()
    {
        foreach (var source in musicSource)
        {
            if (source != null)
            {
                source.volume = musicSlider.value;
            }
        }
    }

    public void OnSfxSliderChanged()
    {
        foreach (var source in sfxSources)
        {
            if (source != null)
            {
                source.volume = sfxSlider.value;
            }
        }
    }

    public void RegisterSfx(AudioSource source)
    {
        if (source != null)
        {
            sfxSources.Add(source);
            source.volume = sfxSlider.value;
        }
    }
    public void RegisterMusic(AudioSource source)
    {
        if (source != null)
        {
            musicSource.Add(source);
            source.volume = musicSlider.value;
        }
    }

    public void Mute()
    {
        foreach (var source in sfxSources)
        {
            if (source != null)
            {
                source.mute = true;
            }
        }
        foreach (var source in musicSource)
        {
            if (source != null)
            {
                source.mute = true;
            }
        }
    }

    public void UnMute()
    {
        foreach (var source in sfxSources)
        {
            if (source != null)
            {
                source.mute = false;
            }
        }
        foreach (var source in musicSource)
        {
            if (source != null)
            {
                source.mute = false;
            }
        }
    }

    public void saveAudio()
    {
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }
}
