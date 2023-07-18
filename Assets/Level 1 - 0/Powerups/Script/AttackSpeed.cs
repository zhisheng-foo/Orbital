using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackSpeed : Collidable
{
    private float cooldown = 0.2f;
    private float lastShout;
    private int dollarRequired = 20;
    public float duration = 15f;
    private float atkspdCooldown = 1.0f;
    private float lastAtkSpd;
    private bool isDestroyed = false;
    private bool isBought = false; 
    private Weapon weapon;
    private Player player;
    private float originalYSpeed;
    private float originalXSpeed;
    public int ogHamDmgPt;
    public AudioSource powerUpAudioSource;
    public AudioSource insufficientDollarAudioSource;
    public AudioSource powerDownAudioSource;
    private bool isShouting = false;
    private float shoutCooldown = 3f;
    private float lastShoutTime;

    
    protected override void Start()
    {
        base.Start();
        weapon = GameManager.instance.weapon;
        player = GameManager.instance.player;

        // Store original values
        originalYSpeed = player.ySpeed;
        originalXSpeed = player.xSpeed;
        
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }

        if (isDestroyed || isBought) 
        {
            return;
        }

        if (GameManager.instance.dollar >= dollarRequired && player.noStackingAtk == false)
        {
            if (Time.time - lastAtkSpd > atkspdCooldown)
            {
                lastAtkSpd = Time.time;

                GameManager.instance.dollar -= dollarRequired;

                GameManager.instance.ShowText("Ham Breathing First Form",
                    20, new Color(0f, 0f, 0f), transform.position + Vector3.up * 0.45f, Vector3.up * 30, 1.0f);
                
                ogHamDmgPt = weapon.damagePoint;
            
                player.ySpeed *= 1.5f;
                player.xSpeed *= 1.5f;
                weapon.damagePoint *= 2;
                player.noStackingAtk = true;
                

                
                powerUpAudioSource.Play();
                player.StartCoroutine(player.ResetPlayerStats(duration, ogHamDmgPt));

                isBought = true; 
                gameObject.SetActive(false);

                Destroy(gameObject);
            }
        }
        else
        {
            if (!isShouting && Time.time - lastShoutTime > shoutCooldown)
            {
                lastShoutTime = Time.time;
                StartCoroutine(ShoutNoStacking());
            }
        }
    }

    IEnumerator ShoutNoStacking()
    {
        isShouting = true;

        if (GameManager.instance.dollar < dollarRequired)
        {
            GameManager.instance.ShowText("More. You are not fit to learn",
                20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 1.0f);

            // Play insufficient dollar audio
            insufficientDollarAudioSource.Play();
        }
        else
        {
            GameManager.instance.ShowText("NO STACKING",
                20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 0.5f);
            insufficientDollarAudioSource.Play();
        }

        yield return new WaitForSeconds(cooldown);

        isShouting = false;
    }
}
