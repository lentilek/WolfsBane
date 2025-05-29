using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip bgDay, bgNight, uiHover, uiAccept, taskMenu, fireOff;

    [SerializeField] private AudioClip taskWood, taskLeaves, taskLakeTrash, taskLakeMeasure, taskRockResearch, taskSalt, taskCamera;

    [SerializeField] private AudioClip[] nightStart, dayStart, uiClose, chicken, dialogue, panicMeter, playerMove, trap, collect, killTurist;

    [HideInInspector] public AudioSource audioSrc;
    public AudioSource audioDialogue, audioAmbient;

    [SerializeField] private float lowerVolume1, lowerVolume2, fadeTime;
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
        audioSrc = GetComponent<AudioSource>();
        PlayAmbient(true);
    }
    public void PlaySound(string clip)
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            audioSrc.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        switch (clip)
        {
            case "night":
                audioSrc.PlayOneShot(nightStart[Random.Range(0, nightStart.Length)], audioSrc.volume * lowerVolume1);
                break;
            case "day":
                audioSrc.PlayOneShot(dayStart[Random.Range(0, dayStart.Length)], audioSrc.volume * lowerVolume1);
                break;
            case "pm":
                audioSrc.PlayOneShot(panicMeter[Random.Range(0, panicMeter.Length)]);
                break;
            case "move":
                audioSrc.PlayOneShot(playerMove[Random.Range(0, playerMove.Length)]);
                break;
            case "kill":
                audioSrc.PlayOneShot(killTurist[Random.Range(0, killTurist.Length)], audioSrc.volume * lowerVolume2);
                break;
            case "chicken":
                audioSrc.PlayOneShot(chicken[Random.Range(0, chicken.Length)]);
                break;
            case "collectWood":
                audioSrc.PlayOneShot(collect[0]);
                break;
            case "collectStone":
                audioSrc.PlayOneShot(collect[1]);
                break;
            case "collectRope":
                audioSrc.PlayOneShot(collect[2]);
                break;
            case "trap":
                audioSrc.PlayOneShot(trap[Random.Range(0, trap.Length)], audioSrc.volume * lowerVolume1);
                break;
            case "uiSound":
                audioSrc.PlayOneShot(uiAccept, audioSrc.volume * lowerVolume1);
                break;
            case "uiClose":
                audioSrc.PlayOneShot(uiClose[Random.Range(0, uiClose.Length)]);
                break;
            case "uiHover":
                audioSrc.PlayOneShot(uiHover, audioSrc.volume * lowerVolume1);
                break;
            case "taskWood":
                audioSrc.PlayOneShot(taskWood, audioSrc.volume * lowerVolume2);
                break;
            case "taskLeaves":
                audioSrc.PlayOneShot(taskLeaves);
                break;
            case "taskSalt":
                audioSrc.PlayOneShot(taskSalt);
                break;
            case "taskCamera":
                audioSrc.PlayOneShot(taskCamera, audioSrc.volume * lowerVolume1);
                break;
            case "taskRockResearch":
                audioSrc.PlayOneShot(taskRockResearch);
                break;
            case "taskMeasure":
                audioSrc.PlayOneShot(taskLakeMeasure);
                break;
            case "taskTrash":
                audioSrc.PlayOneShot(taskLakeTrash);
                break;
            case "taskMenu":
                audioSrc.PlayOneShot(taskMenu);
                break;
            case "fireOff":
                audioSrc.PlayOneShot(fireOff);
                break;
            default: 
                break;
        }
    }
    public void DialogueSound()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            audioDialogue.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        audioDialogue.clip = dialogue[Random.Range(0, dialogue.Length)];
        audioDialogue.Play();
    }
    public void PlayAmbient(bool dayType)
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            audioAmbient.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        if (audioAmbient.clip == null) audioAmbient.clip = bgDay;
        else
        {
            if (dayType) StartCoroutine(FadeOut(audioAmbient, fadeTime, bgDay));
            else StartCoroutine(FadeOut(audioAmbient, fadeTime, bgNight));
        }
    }
    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime, AudioClip clip)
    {
        float startVolume = audioSource.volume;

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
