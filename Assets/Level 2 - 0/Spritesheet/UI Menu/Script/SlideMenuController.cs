using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideMenuController : MonoBehaviour
{
    public Animator menuAnimator;
    public Button button;
    public AudioSource audioSource;

    private bool isMenuVisible = false;
    private bool isGamePaused = false;
    private bool isTransitioning = false;  

    private void Start()
    {
        button.onClick.AddListener(ToggleMenu);
        button.interactable = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isTransitioning)  
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (isTransitioning)
            return;  

        isMenuVisible = !isMenuVisible;

        if (isMenuVisible)
        {
            StartCoroutine(PauseGameAndShowMenuAfterDelay(0.4f)); 

            // Play the audio clip
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            menuAnimator.SetTrigger("hide"); 
            Time.timeScale = 1f; 
            isGamePaused = false;
        }
    }

    private IEnumerator PauseGameAndShowMenuAfterDelay(float delay)
    {
        menuAnimator.SetTrigger("show");  
        isTransitioning = true;  

        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;  
        isGamePaused = true;

        isTransitioning = false;  
    }
}
