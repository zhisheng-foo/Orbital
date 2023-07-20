using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
Handles the death screen canvas when player dies,
as well as increasing player weapon damage by 1,
resetting the player weapon when entering new level such as: 1-1 to 2-1 if player has died
through the use of bools which is initialised in the player class.
*/



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

    public Weapon weapon;
    public GameManager gamemanager;



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

    }
    
    //Called when player hitpoint reaches 0. Enables the canvas and starts the fade to black.
    public void GameOver()
    {

        gameOverCanvas.SetActive(true);
        
        Time.timeScale = 0;

        backgroundAudioSources = GameObject.Find("GameSound");
        StartCoroutine(FadeOverlay());
        PlayGameOverSound();
        StopBackgroundAudio();
    }

    //Part of GameOver(), pauses the game immediately.
    private IEnumerator FadeOverlay()
    {
        float elapsedTime = 0f;
        Color mainColor = fadeOverlayMain.color;
        Color txtColor = fadeOverlayTxt.color;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
           
            float mainAlpha = Mathf.Lerp(0f, 1f, elapsedTime/fadeDuration);
            float txtAlpha = Mathf.Lerp(0f, 1f, elapsedTime/(fadeDuration * 1.5f));
  
            mainColor.a = mainAlpha;
            txtColor.a = txtAlpha;

            fadeOverlayMain.color = mainColor;
            fadeOverlayTxt.color = txtColor;
            yield return null;
        }
    }

    //Start the death sound.
    private void PlayGameOverSound()
    {
        audioSource.Play();
    }
    
    //Stop any BGM that is still palying.
    private void StopBackgroundAudio()
    {
        Destroy(backgroundAudioSources);
    }

    //Button to "respawn" the player at the beginning of the level. 
    //Example: 2-1 will respawn at 2-0 with +1 increased weapon damage
    public void RestartGame()
    {
        Debug.Log("Game Restart");
          
        player.hitpoint = player.maxHitpoint;
        player.isPlayerDead = false;
        gameOverCanvas.SetActive(false);

        //reloading current scene
        Scene currentScn = SceneManager.GetActiveScene();
        if (currentScn.name == "Level 1 - 0" || currentScn.name == "Level 1 - 1")
        {
            SceneManager.LoadScene("Level 1 - 0");
            weapon.damagePoint += 1;
            player.atkbuffed1 = true;
            gamemanager.ShowText("+1 attack cuz u noob", 25, new Color(0f, 255f, 0f), 
                player.transform.position, Vector3.up * 20, 1.5f);

        }
        else if (currentScn.name == "Level 2 - 0")
        {
            SceneManager.LoadScene("Level 2 - 0");
            gamemanager.ShowText("Wai u die?", 25, new Color(0f, 255f, 0f), 
                player.transform.position, Vector3.up * 20, 2f);
            
        }
        else if(currentScn.name == "Level 2 - 1")
        {
            SceneManager.LoadScene("Level 2 - 0");
            weapon.damagePoint += 1;
            player.atkbuffed2 = true;
            gamemanager.ShowText("+1 attack cuz u noob", 25, new Color(0f, 255f, 0f), 
                player.transform.position, Vector3.up * 20, 1.5f);
            
        }
        else if (currentScn.name == "Level 3 - 0")
        {
            SceneManager.LoadScene("Level 3 - 0");
            weapon.damagePoint += 1;
            player.atkbuffed3 = true;
            gamemanager.ShowText("+1 attack, u have 2 new skills cuz boggo just rmbred.", 25, new Color(0f, 255f, 0f), 
                player.transform.position, Vector3.up * 20, 3f);
            
        }
        else
        {
            SceneManager.LoadScene("Start Game");
            weapon.damagePoint = 2;
        }
        Time.timeScale = 1f;     
    }
}
