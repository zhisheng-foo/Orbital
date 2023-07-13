using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotArmour : Collidable
{
    private float cooldown = 1.5f;
    private float lastShout;
    private bool canShowNoStackingMessage = true;

    private int dollarRequired = 20;

    public int duration = 10;

    private float plotCooldown = 1.0f;
    private float lastArmour;

    private bool isDestroyed = false;
    private bool isBought = false; // Track if the object has been bought

    public AudioSource powerUpAudioSource;
    public AudioSource powerUpEndAudioSource;
    public AudioSource insufficientFundsAudioSource;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }

        Player player = GameManager.instance.player;

        if (isDestroyed || isBought) // Check if the object has been bought
        {
            return;
        }

        if (GameManager.instance.dollar >= dollarRequired && !player.noStackingplot)
        {
            if (Time.time - lastArmour > plotCooldown)
            {
                lastArmour = Time.time;

                GameManager.instance.dollar -= dollarRequired;
                player.noStackingplot = true;

                GameManager.instance.ShowText("Plot Armour activated", 20, new Color(0f, 0f, 0f), transform.position + Vector3.up * 0.45f, Vector3.up * 30, 1.0f);
                player.StartCoroutine(ActivateInvulnerabilityAndDestroyObject(duration));

                // Play power-up audio
                powerUpAudioSource.Play();

                isBought = true; // Set the object as bought
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (canShowNoStackingMessage && Time.time - lastShout > cooldown)
            {
                lastShout = Time.time;

                if (GameManager.instance.dollar < dollarRequired)
                {
                    GameManager.instance.ShowText("Prove yourself. You are not the protagonist.", 20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 1.0f);

                    // Play insufficient funds audio
                    insufficientFundsAudioSource.Play();
                }
                else
                {
                    GameManager.instance.ShowText("NO STACKING", 20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 1.0f);
                    canShowNoStackingMessage = false; // Prevent showing the message until cooldown ends

                    // Play insufficient funds audio
                    insufficientFundsAudioSource.Play();
                    StartCoroutine(ResetNoStackingMessageCooldown());
                }
            }
        }
    }

    IEnumerator ActivateInvulnerabilityAndDestroyObject(float duration)
    {
        Player player = GameManager.instance.player;
        player.isInvulnerable = true;

        // Hide the object
        gameObject.SetActive(false);

        yield return new WaitForSeconds(duration);

        player.isInvulnerable = false;
        player.noStackingplot = false;

         Vector3 textPosition = player.transform.position + Vector3.up * 0.25f;
        GameManager.instance.ShowText("Plot Armour Deactivated", 20, new Color(0f, 0f, 0f), textPosition, Vector3.up , 1.0f);

        yield return new WaitForSeconds(1.0f);
        // Play power-up end audio
        powerUpEndAudioSource.Play();

       

        // Destroy the object after the power-up duration
        Destroy(gameObject);
    }

    IEnumerator ResetNoStackingMessageCooldown()
    {
        yield return new WaitForSeconds(cooldown);

        canShowNoStackingMessage = true; // Reset the cooldown to allow showing the message again
    }
}
