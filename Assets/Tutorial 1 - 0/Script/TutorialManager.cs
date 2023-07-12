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
            playedFirstSong = false; // Reset the flag to prevent playing the second song repeatedly      
        }
        
        StartCoroutine(LoadStartGameSceneAfterDelay()); // Start the coroutine to load the Start Game scene
    }

    IEnumerator LoadStartGameSceneAfterDelay()
    {
        yield return new WaitForSeconds(42.0f);

        // Gradually decrease the volume over time
        float fadeDuration = 1.0f; // Adjust the duration as per your preference
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
        SceneManager.LoadScene("Start Game"); // Load the Start Game scene
    }
}
