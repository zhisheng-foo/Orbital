using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;

    private void Start()
    {
        // Disable the game over canvas at the beginning
        gameOverCanvas.SetActive(false);
    }

    // This method is called when the player's HP reaches 0
    public void GameOver()
    {
        // Enable the game over canvas
        gameOverCanvas.SetActive(true);
        // Pause the game
        Time.timeScale = 0f;
    }
}
