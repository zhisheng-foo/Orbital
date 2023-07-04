using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Image fadeOverlayMain;
    public Image fadeOverlayTxt;
    public float fadeDuration = 5f;
    public AudioClip gameOverSound;
    
    private AudioSource audioSource;
    private GameObject backgroundAudioSources;

    private void Start()
    {
        gameOverCanvas.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameOverSound;

        //Find all AudioSources in the scene and store them in the background
        backgroundAudioSources = GameObject.Find("GameSound");
    }

    
    public void GameOver()
    {
        // Enable the game over canvas
        gameOverCanvas.SetActive(true);
        // Pause the game
        //Time.timeScale = 0f;
        StartCoroutine(FadeOverlay());
        PlayGameOverSound();
        StopBackgroundAudio();
    }

    private IEnumerator FadeOverlay()
    {
        float elapsedTime = 0f;
        Color mainColor = fadeOverlayMain.color;
        Color txtColor = fadeOverlayTxt.color;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            // Calculate alpha values for main overlay and text overlay separately
            float mainAlpha = Mathf.Lerp(0f, 1f, elapsedTime/fadeDuration);
            float txtAlpha = Mathf.Lerp(0f, 1f, elapsedTime/(fadeDuration * 1.5f));

            //Updating alpha values for main overlay and text overlay
            mainColor.a = mainAlpha;
            txtColor.a = txtAlpha;

            //Apply Updated colors to main overlay and text overlay
            fadeOverlayMain.color = mainColor;
            fadeOverlayTxt.color = txtColor;
            yield return null;
        }
    }

    private void PlayGameOverSound()
    {
        audioSource.Play();
    }
    
    private void StopBackgroundAudio()
    {
        /*
        foreach(AudioSource bgAudioSource in backgroundAudioSources)
        {
            if(bgAudioSource != audioSource)
            {
                bgAudioSource.Stop();
            }
        }
        */
        Destroy(backgroundAudioSources);
    }

    public void RestartGame()
    {
        Debug.Log("Game Restart");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Game");
    }
}
