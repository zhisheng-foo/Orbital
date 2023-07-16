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
    private bool isBought = false; 
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

        if (isDestroyed || isBought) 
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

                GameManager.instance.ShowText("Plot Armour activated", 20, new Color(0f, 0f, 0f),
                 transform.position + Vector3.up * 0.45f, Vector3.up * 30, 1.0f);
                player.StartCoroutine(ActivateInvulnerabilityAndDestroyObject(duration));

                powerUpAudioSource.Play();

                isBought = true;
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
                    GameManager.instance.ShowText("Prove yourself. You are not the protagonist.",
                     20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 1.0f);

                    insufficientFundsAudioSource.Play();
                }
                else
                {
                    GameManager.instance.ShowText("NO STACKING", 20, new Color(0f, 0f, 0f),
                     transform.position, Vector3.up * 20, 1.0f);
                    canShowNoStackingMessage = false;

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

        gameObject.SetActive(false);

        yield return new WaitForSeconds(duration);

        player.isInvulnerable = false;
        player.noStackingplot = false;

        Vector3 textPosition = player.transform.position + Vector3.up * 0.25f;
        GameManager.instance.ShowText("Plot Armour Deactivated",
         20, new Color(0f, 0f, 0f), textPosition, Vector3.up , 1.0f);

        yield return new WaitForSeconds(1.0f);
        
        powerUpEndAudioSource.Play();

        Destroy(gameObject);
    }

    IEnumerator ResetNoStackingMessageCooldown()
    {
        yield return new WaitForSeconds(cooldown);

        canShowNoStackingMessage = true;
    }
}
