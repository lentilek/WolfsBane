using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [SerializeField] private Slider sfxSlider;
    private float sfxVolume;
    [SerializeField] private Slider musicSlider;
    private float musicVolume;

    [SerializeField] private GameObject xMusic, xSFX;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }
    public void Open()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusic();
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        SFXVolume();
        MusicVolume();
    }
    public void SFXVolume()
    {
        sfxVolume = sfxSlider.value;
        if (sfxVolume != 0)
        {
            xSFX.SetActive(false);
        }
        else
        {
            xSFX.SetActive(true);
        }
        AudioManager.Instance.audioSrc.volume = sfxVolume;
        AudioManager.Instance.audioAmbient.volume = sfxVolume;
        AudioManager.Instance.audioDialogue.volume = sfxVolume;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }
    public void MusicVolume()
    {
        musicVolume = musicSlider.value;
        if (musicVolume != 0)
        {
            xMusic.SetActive(false);
        }
        else
        {
            xMusic.SetActive(true);
        }
        MusicPlayer.Instance.audioSrc.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    public void MusicVolumeOff()
    {
        if (musicVolume != 0)
        {
            musicVolume = 0;
            musicSlider.value = 0;
            xMusic.SetActive(true);
        }
        else
        {
            musicVolume = 1f;
            musicSlider.value = 1f;
            xMusic.SetActive(false);
        }
        MusicPlayer.Instance.audioSrc.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    public void SFXVolumeOff()
    {
        if (sfxVolume != 0)
        {
            sfxVolume = 0;
            sfxSlider.value = 0;
            xSFX.SetActive(true);
        }
        else
        {
            sfxVolume = 1f;
            sfxSlider.value = 1f;
            xSFX.SetActive(false);
        }
        AudioManager.Instance.audioSrc.volume = sfxVolume;
        AudioManager.Instance.audioAmbient.volume = sfxVolume;
        AudioManager.Instance.audioDialogue.volume = sfxVolume;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }
    public void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        MusicVolume();
    }
    public void LoadVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SFXVolume();
    }
    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
    public void UIHover()
    {
        AudioManager.Instance.PlaySound("uiHover");
    }
    public void UIClose()
    {
        AudioManager.Instance.PlaySound("uiClose");
    }
}
