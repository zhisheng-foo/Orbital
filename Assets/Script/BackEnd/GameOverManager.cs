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
    public Player player;


    private AudioSource audioSource;
    private GameObject backgroundAudioSources;

    private static GameOverManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverCanvas.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameOverSound;

        //Find all AudioSources in the scene and store them in the background
    }

    
    public void GameOver()
    {
        // Enable the game over canvas
        gameOverCanvas.SetActive(true);
        // Pause the game
        Time.timeScale = 0;

        backgroundAudioSources = GameObject.Find("GameSound");
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
        Destroy(backgroundAudioSources);
    }

    public void RestartGame()
    {
        Debug.Log("Game Restart");
        Time.timeScale = 1f;
        
        
        player.hitpoint = player.maxHitpoint;
        player.isDead = false;
        gameOverCanvas.SetActive(false);


        Scene currentScn = SceneManager.GetActiveScene();

        if(currentScn.name == "Level 1 - 0" || currentScn.name == "Level 1 - 1")
        {
            SceneManager.LoadScene("Level 1 - 0");
        }

        else if(currentScn.name == "Level 2 - 0" || currentScn.name == "Level 2 - 1")
        {
            SceneManager.LoadScene("Level 2 - 0");
        }

        else if(currentScn.name == "Level 3 - 0")
        {
            SceneManager.LoadScene("Level 3 - 0");
        }

        else
        {
            SceneManager.LoadScene("Start Game");
        }
        
    }
}
