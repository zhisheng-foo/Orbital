using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Handles the healing logic of the doughnut to only heal the player
by at most 5 hitpoints
*/

public class DoughnutHealth : Collidable
{
    private float healCooldown = 1.0f;
    private float lastHeal;
    private float cooldown = 1.5f;
    private float lastShout;
    private int dollarRequired = 20;
    public AudioClip healAudioClip;
    public AudioClip notEnoughDollarAudioClip;
    private Player player; 
    public float healAudioVolume = 1.0f;
    public float notEnoughDollarAudioVolume = 1.0f;
    private AudioSource audioSource;
    private bool isDestroyed = false;
    private bool healMessageShown = false;
    private void Start()
    {
        base.Start();   
        audioSource = GetComponent<AudioSource>();  
        audioSource.volume = healAudioVolume;
        player = GameObject.Find("Player").GetComponent<Player>();
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
                healMessageShown = true;
            
                int healingAmount = Mathf.Clamp(maxHealth - currentHealth, 1, 5);

                player.Heal(healingAmount);

                GameManager.instance.dollar -= dollarRequired;

                StartCoroutine(PlayAudioAndDestroy(healAudioClip, () =>
                {
                    DestroyDoughnut();
                    healMessageShown = true;
                }));
            }
        }
        else
        {
            if (!healMessageShown && Time.time - lastShout > cooldown)
            {
                lastShout = Time.time;

                audioSource.PlayOneShot(notEnoughDollarAudioClip);

                if (GameManager.instance.dollar < dollarRequired)
                {
                    GameManager.instance.ShowText("The chosen demands more", 20,
                     new Color(0.0f, 0.0f, 0.0f), transform.position, Vector3.up * 30, 1.0f);
                }
                else
                {
                    lastShout = Time.time - cooldown; 
                }
            }
            else if (healMessageShown)
            {
                lastShout = Time.time - cooldown;
            }
        }
    }
    private IEnumerator PlayAudioAndDestroy(AudioClip clip, System.Action callback, float delay = 0.0f)
    {     
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length + delay);
        callback.Invoke();
    }

    private void DestroyDoughnut()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
