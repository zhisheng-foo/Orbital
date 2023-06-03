using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughnutHealth : Collidable
{
    private float healCooldown = 1.0f;
    private float lastHeal;

    private float cooldown = 1.5f;
    private float lastShout;

    private int dollarRequired = 10;

    public AudioClip healAudioClip;
    public AudioClip notEnoughDollarAudioClip;

    private Player player; // Reference to the Player component in the current scene

    public float healAudioVolume = 1.0f;
    public float notEnoughDollarAudioVolume = 1.0f;

    private AudioSource audioSource;

    private bool isDestroyed = false;

    private void Start()
    {
        base.Start();

        // Get reference to the AudioSource
        audioSource = GetComponent<AudioSource>();

        // Set the initial volume
        audioSource.volume = healAudioVolume;

        // Find the Player object in the current scene
        player = FindObjectOfType<Player>();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }

        int maxHealth = player.maxHitpoint;
        int currentHealth = player.hitpoint;

        if (isDestroyed)
        {
            return;
        }

        if (GameManager.instance.dollar >= dollarRequired)
        {
            if (Time.time - lastHeal > healCooldown)
            {
                lastHeal = Time.time;

                // Calculate the healing amount based on the player's current health
                int healingAmount = Mathf.Clamp(maxHealth - currentHealth, 1, 5);

                // Apply the healing to the player
                player.Heal(healingAmount);

                GameManager.instance.dollar -= dollarRequired;

                // Play the heal audio clip and register a callback for when it finishes playing
                StartCoroutine(PlayAudioAndDestroy(healAudioClip, DestroyDoughnut));
            }
        }
        else
        {
            if (Time.time - lastShout > cooldown)
            {
                lastShout = Time.time;

                // Play the not enough dollar audio clip
                audioSource.PlayOneShot(notEnoughDollarAudioClip);

                if (GameManager.instance.dollar < dollarRequired)
                {
                    GameManager.instance.ShowText("The chosen demands more", 20, new Color(0.8f, 0.0f, 0.0f), transform.position, Vector3.up * 30, 1.0f);
                }
                else
                {
                    lastShout = Time.time - cooldown; // Reset the lastShout time to avoid showing the message immediately after buying the doughnut
                }
            }
        }
    }

    private IEnumerator PlayAudioAndDestroy(AudioClip clip, System.Action callback, float delay = 0.0f)
    {
        // Play the audio clip
        audioSource.PlayOneShot(clip);

        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(clip.length + delay);

        // Call the callback function
        callback.Invoke();
    }

    private void DestroyDoughnut()
    {
        isDestroyed = true;

        // Destroy the game object
        Destroy(gameObject);
    }
}
