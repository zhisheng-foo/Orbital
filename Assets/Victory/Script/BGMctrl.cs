using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//handles the BGM of the Victory Scene
public class BGMctrl : MonoBehaviour
{
    public AudioClip BGM;
    public AudioClip spider;
    private AudioSource audiosource;
    private bool isPaused = false;
    private float pauseTime = 11.3f;
    private float currentTime = 0f;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioClips());
    }

    IEnumerator PlayAudioClips()
    {
        audiosource.clip = spider;
        audiosource.Play();

        yield return new WaitForSeconds(spider.length + 1.0f);

        audiosource.clip = BGM;
        audiosource.Play();

        while (currentTime < pauseTime)
        {
            if (!isPaused)
                currentTime += Time.deltaTime;

            yield return null;
        }

        audiosource.Pause();

        while (audiosource.time < BGM.length)
        {
            if (!isPaused)
                audiosource.UnPause();
            else
                audiosource.Pause();

            yield return null;
        }
    }

    public void PauseResumeAudio()
    {
        isPaused = !isPaused;

        if (isPaused)
            audiosource.Pause();
        else
            audiosource.UnPause();
    }

    void Update()
    {
        
    }
}
