using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackSpeed : Collidable
{
    private float cooldown = 1.5f;
    private float lastShout;

    private int dollarRequired = 20;

    public float duration = 15f;

    private float atkspdCooldown = 1.0f;
    private float lastAtkSpd;

    private bool isDestroyed = false;
    private bool isBought = false; // Track if the object has been bought

    private Weapon weapon;
    private Player player;

    // Variables to store original values
    private float originalYSpeed;
    private float originalXSpeed;
    private int originalDamagePoint;

    public AudioSource powerUpAudioSource;
    public AudioSource insufficientDollarAudioSource;

    public AudioSource powerDownAudioSource;

    protected override void Start()
    {
        base.Start();
        weapon = GameManager.instance.weapon;
        player = GameManager.instance.player;

        // Store original values
        originalYSpeed = player.ySpeed;
        originalXSpeed = player.xSpeed;
        originalDamagePoint = weapon.damagePoint;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }

        if (isDestroyed || isBought) // Check if the object has been bought
        {
            return;
        }

        if (GameManager.instance.dollar >= dollarRequired)
        {
            if (Time.time - lastAtkSpd > atkspdCooldown)
            {
                lastAtkSpd = Time.time;

                GameManager.instance.dollar -= dollarRequired;

                GameManager.instance.ShowText("Ham Breathing First Form",
                    20, new Color(0.7f, 0.2f, 0f), transform.position + Vector3.up * 0.45f, Vector3.up * 30, 1.0f);

                // Increase player's speed and attack
                player.ySpeed *= 1.5f;
                player.xSpeed *= 1.5f;
                weapon.damagePoint *= 2;

                // Play power-up audio
                powerUpAudioSource.Play();

                // Start coroutine to reset player's speed and attack after duration
                player.StartCoroutine(player.ResetPlayerStats(duration));
               
                isBought = true; // Set the object as bought
                gameObject.SetActive(false);
                
                Destroy(gameObject);
            }
        }
        else
        {
            if (Time.time - lastShout > cooldown)
            {
                lastShout = Time.time;

                if (GameManager.instance.dollar < dollarRequired)
                {
                    GameManager.instance.ShowText("More. You are not fit to learn",
                        20, new Color(0.7f, 0.2f, 0f), transform.position, Vector3.up * 20, 1.0f);

                    // Play insufficient dollar audio
                    insufficientDollarAudioSource.Play();
                }
                else
                {
                    lastShout = Time.time - cooldown; // Reset the lastShout time to avoid showing the message immediately after buying the doughnut
                }
            }
        }
    }


}

