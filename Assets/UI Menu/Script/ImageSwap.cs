using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour
{
    public KeyCode swapKeyCode;
    public Sprite newImage;
    private Sprite originalImage;
    private Image image;
    private bool isSwapping = false;
    private float swapStartTime;
    private AudioSource playerAudioSource; // Reference to the player's AudioSource component
    private bool isMenuVisible = false; // Flag to track menu visibility

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on the GameObject!");
            enabled = false; // Disable the script
            return;
        }

        originalImage = image.sprite;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Weapon");
        if (playerObject != null)
        {
            playerAudioSource = playerObject.GetComponent<AudioSource>();
            if (playerAudioSource == null)
            {
                Debug.LogWarning("Player's AudioSource component not found!");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(swapKeyCode) && !isSwapping)
        {
            image.sprite = newImage;
            isSwapping = true;
            swapStartTime = Time.unscaledTime;

            if (isMenuVisible && playerAudioSource != null)
            {
                playerAudioSource.enabled = false; // Disable the player's AudioSource component
            }
        }

        if (isSwapping)
        {
            float elapsedTime = Time.unscaledTime - swapStartTime;
            if (elapsedTime >= 0.1f)
            {
                image.sprite = originalImage;
                isSwapping = false;

                if (isMenuVisible && playerAudioSource != null)
                {
                    playerAudioSource.enabled = true; // Re-enable the player's AudioSource component
                }
            }
        }
    }

    // Method to set the menu visibility state
    public void SetMenuVisible(bool visible)
    {
        isMenuVisible = visible;
        if (isMenuVisible && playerAudioSource != null)
        {
            playerAudioSource.enabled = false; // Disable the player's AudioSource component
        }
        else if (!isMenuVisible && playerAudioSource != null && !isSwapping)
        {
            playerAudioSource.enabled = true; // Re-enable the player's AudioSource component
        }
    }
}
