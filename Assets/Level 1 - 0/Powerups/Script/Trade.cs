using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : Collidable
{
    private float cooldown = 1.5f;
    private float lastShout;
    public float duration = 10f;
    private int healthRequired = 5;
    private float tradeCooldown = 1.0f;
    private float lastTrade;
    private bool isDestroyed = false;
    private bool isBought = false; 
    private Weapon weapon;
    private Player player;
    public AudioSource powerUpAudioSource;
    public AudioSource insufficientHealthAudioSource;

    public int ogCeleryDmgPt; 

    protected override void Start()
    {
        base.Start();
        weapon = GameManager.instance.weapon;
        player = GameManager.instance.player;
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

        if (player.hitpoint > healthRequired) 
        {
            lastTrade = Time.time;
            player.hitpoint -= healthRequired;

             GameManager.instance.ShowText("The curse has been placed   ",
                    20, new Color(0f, 0f, 0f), transform.position, 
                    Vector3.up * 20, 1.0f);
            ogCeleryDmgPt = weapon.damagePoint;
            weapon.damagePoint += 999;
            powerUpAudioSource.Play();

            player.StartCoroutine(player.ResetPlayerStatsTrade(duration, ogCeleryDmgPt));
            isBought = true; 
            gameObject.SetActive(false);
                
            Destroy(gameObject);
        }

        else
        {
            if (Time.time - lastShout > cooldown)
            {
                lastShout = Time.time;

                if (player.hitpoint <= healthRequired)
                {
                    GameManager.instance.ShowText("Not enough hp to sacrifice",
                        20, new Color(0f, 0f, 0f), transform.position, Vector3.up * 20, 1.0f);

                    insufficientHealthAudioSource.Play();
                }
                else
                {
                    lastShout = Time.time - cooldown; 
                }
            }
        }
    }
}
