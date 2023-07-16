using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Sound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip bossMusic;
    public AudioClip finalMusic;
    private bool isBossWave = false;
    private GameObject waveManager;
    private int counter = 0;

    void Start()
    {
        audioSource.clip = startMusic;
        audioSource.loop = true;
        audioSource.Play();
    
        waveManager = GameObject.Find("Wave3Manager(Clone)");
    }

    void Update()
    {   
        waveManager = GameObject.Find("Wave3Manager(Clone)");
        if (!isBossWave && IsBossWave())
        {
            isBossWave = true;
            StartCoroutine(CrossfadeMusic(bossMusic));
        }

        if (isBossWave && IsBossWaveDone() && counter != 1)
        {
            StartCoroutine(CrossfadeMusic(finalMusic));
            counter++;
        }
    }

    bool IsBossWave()
    {
        return waveManager != null;
    }

    bool IsBossWaveDone()
    {
        return waveManager == null;
    }


        IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        float fadeDuration = 2f;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.volume = 0.775f;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

}
