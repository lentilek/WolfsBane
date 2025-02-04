using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip collect, trap, uiSound;

    [HideInInspector] public AudioSource audioSrc;
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
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "collect":
                audioSrc.PlayOneShot(collect);
                break;
            case "trap":
                audioSrc.PlayOneShot(trap);
                break;
            case "uiSound":
                audioSrc.PlayOneShot(uiSound);
                break;
            default: 
                break;
        }
    }
}
