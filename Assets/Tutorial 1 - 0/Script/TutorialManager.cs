using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public AudioClip GoodMorningVietnam;
    public AudioClip AnotherSong;
    private AudioSource audiosource;
    private bool playedFirstSong;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(GoodMorningVietnam);
        playedFirstSong = true;
    }

    void Update()
    {
        if (playedFirstSong && !audiosource.isPlaying)
        {   
            audiosource.clip = AnotherSong;
            audiosource.loop = true;
            audiosource.volume = 0.56f;
            audiosource.Play();
            playedFirstSong = false;     
        }    
        StartCoroutine(LoadStartGameSceneAfterDelay());
    }

    IEnumerator LoadStartGameSceneAfterDelay()
    {
        yield return new WaitForSeconds(42.0f);

        float fadeDuration = 1.0f; 
        float startVolume = audiosource.volume;
        float startTime = Time.time;

        while (Time.time - startTime <= fadeDuration)
        {
            float elapsed = Time.time - startTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            audiosource.volume = newVolume;
            yield return null;
        }
        audiosource.volume = 0f;

        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("Start Game"); 
    }
}
