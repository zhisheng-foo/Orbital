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

    // Start is called before the first frame update
    void Start()
    {
        // Play the start music
        audioSource.clip = startMusic;
        audioSource.loop = true;
        audioSource.Play();
        
        // Find the "Wave3Manager" clone
        waveManager = GameObject.Find("Wave3Manager(Clone)");
    }

    // Update is called once per frame
    void Update()
    {   
        waveManager = GameObject.Find("Wave3Manager(Clone)");
        // Check if it is the Boss Wave and switch to boss music
        if (!isBossWave && IsBossWave())
        {
            isBossWave = true;
            StartCoroutine(CrossfadeMusic(bossMusic));
        }

        // Check if the Boss Wave is done and switch to final music
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

        // Fade out the current music
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();

        // Switch to the new music, set volume to 0, and start playing
        audioSource.clip = newClip;
        audioSource.volume = 0.775f;
        audioSource.Play();

        // Fade in the new music
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

}
