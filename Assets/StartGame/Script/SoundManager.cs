using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        if (instance != null && clip != null && instance.audioSource != null)
        {
            instance.audioSource.PlayOneShot(clip);
        }
    }
}
