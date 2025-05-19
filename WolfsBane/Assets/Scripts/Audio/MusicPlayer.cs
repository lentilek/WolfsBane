using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [SerializeField] private AudioClip musicMenu, musicDay, musicNight;
    [SerializeField] private float fadeTime;
    [SerializeField] public AudioSource audioSrc;
    private float startVolume;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    public void ChangeMusic(int musicVariant)
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            audioSrc.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        StopAllCoroutines();
        if (audioSrc.clip != null)
        {
            switch (musicVariant)
            {
                case 0:
                    StartCoroutine(FadeOut(audioSrc, fadeTime, musicMenu));
                    break;
                case 1:
                    StartCoroutine(FadeOut(audioSrc, fadeTime, musicDay));
                    break;
                case 2:
                    StartCoroutine(FadeOut(audioSrc, fadeTime, musicNight));
                    break;
                default: break;
            }
        }
        else
        {
            audioSrc.clip = musicMenu;
            audioSrc.Play();
        }
    }
    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime, AudioClip clip)
    {
        startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.clip = clip;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }
}
