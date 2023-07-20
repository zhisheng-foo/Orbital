using UnityEngine;

/* 
Visual element of the portal for 1 - 0.
Controls the rate at which the portal shrinks
*/

public class Shrinking : MonoBehaviour
{
    public float shrinkSpeed = 0.5f; // Adjust this value to control the speed of shrinking
    public AudioClip shrinkSound;
    public float initialVolume = 1f;

    private AudioSource audioSource;
    private Vector3 initialScale;
    private bool isPlaying;

    private void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;

        // Add an AudioSource component and configure it
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = shrinkSound;
        audioSource.volume = initialVolume;

        // Play the sound
        audioSource.Play();
        isPlaying = true;
    }

    private void Update()
    {
        // Calculate the new scale based on the shrink speed and time
        float newScale = Mathf.Lerp(transform.localScale.x, 0f, Time.deltaTime * shrinkSpeed);

        // Apply the new scale to all axes
        transform.localScale = new Vector3(newScale, newScale, newScale);

        // Update the volume based on the current scale
        audioSource.volume = initialVolume * (transform.localScale.x / initialScale.x);

        if (transform.localScale.x <= 0.4f)
        {
            // Stop the sound and destroy the game object
            if (isPlaying)
            {
                audioSource.Stop();
                isPlaying = false;
            }

            Destroy(gameObject);
        }
    }
}
