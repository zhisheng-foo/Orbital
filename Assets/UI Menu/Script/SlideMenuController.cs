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
    private bool isTransitioning = false;  // Flag to track menu transition

    private void Start()
    {
        button.onClick.AddListener(ToggleMenu);
        button.interactable = false;

        // Get the AudioSource component from the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isTransitioning)  // Check if not currently transitioning
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (isTransitioning)
            return;  // Return early if already transitioning

        isMenuVisible = !isMenuVisible;

        if (isMenuVisible)
        {
            StartCoroutine(PauseGameAndShowMenuAfterDelay(0.4f));  // Start the delay

            // Play the audio clip
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            menuAnimator.SetTrigger("hide");  // Hide the menu
            Time.timeScale = 1f;  // Unpause the game
            isGamePaused = false;
        }
    }

    private IEnumerator PauseGameAndShowMenuAfterDelay(float delay)
    {
        menuAnimator.SetTrigger("show");  // Show the menu
        isTransitioning = true;  // Set transitioning flag

        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;  // Pause the game
        isGamePaused = true;

        isTransitioning = false;  // Reset transitioning flag
    }
}
