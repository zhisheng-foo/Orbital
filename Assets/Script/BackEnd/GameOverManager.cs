using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Image fadeOverlay;
    public float fadeDuration = 5f;
    public AudioClip gameOverSound;
    
    private AudioSource audioSource;

    private void Start()
    {
        gameOverCanvas.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameOverSound;
    }

    
    public void GameOver()
    {
        // Enable the game over canvas
        gameOverCanvas.SetActive(true);
        // Pause the game
        Time.timeScale = 0f;
        StartCoroutine(FadeOverlay());
        audioSource.Play();
    }

    private IEnumerator FadeOverlay()
    {
        float elapsedTime = 0f;
        Color currentColor = fadeOverlay.color;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime/fadeDuration);
            currentColor.a = alpha;
            fadeOverlay.color = currentColor;
            yield return null;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Game Restart");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Game");
    }
}
