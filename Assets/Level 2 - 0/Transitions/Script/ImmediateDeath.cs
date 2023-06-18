using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImmediateDeath : Collidable
{
    private Player player;
    public AudioSource audioSource;
    private bool hasPlayedAudio;
    public TextMeshProUGUI textMesh; // Public TextMeshProUGUI object reference
    public float displayDuration = 2f; // Duration in seconds to display the text

    public string text;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        hasPlayedAudio = false;

        // Hide the TextMeshProUGUI initially
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player" && player != null)
        {
            // Reduce player's hitpoints to zero
            player.hitpoint = 0;

            // Play audio upon collision if it hasn't been played before
            if (audioSource != null && !hasPlayedAudio)
            {
                audioSource.Play();
                hasPlayedAudio = true;

                // Display the text on the TextMeshProUGUI object
                DisplayText();
            }
        }
    }

    private void DisplayText()
    {
        if (textMesh != null)
        {
            // Activate the TextMeshProUGUI object
            textMesh.gameObject.SetActive(true);
            textMesh.text = text; // Set the text to display

            // Destroy the TextMeshProUGUI object after the display duration
            StartCoroutine(DestroyTextAfterDelay(displayDuration));
        }
    }

    private IEnumerator DestroyTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (textMesh != null)
        {
            // Deactivate and destroy the TextMeshProUGUI object
            textMesh.gameObject.SetActive(false);
            Destroy(textMesh.gameObject);
        }
    }
}
