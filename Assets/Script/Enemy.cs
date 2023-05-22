using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //Experience
    public int xpValue = 1;

    //Logic 
    public float triggerLength = 0.3f;
    public float chaseLength = 0.5f;

    public bool isDead = false;

    private bool chasing;
    private bool collidingWithPlayer;

    private Transform playerTransform;
    private Vector3 startingPosition;

    public ContactFilter2D filter;
    private BoxCollider2D hitbox;

    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
         
        playerTransform = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        FixedUpdate();
        
    }

    private void FixedUpdate()
    {    
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) 
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength) {
                
                chasing = true;
            }

            if (chasing) 
            {
                if (!collidingWithPlayer) {
                    
                    UpdateMotor((playerTransform.position - transform.position).normalized);

                }
            }
            else 
            {
                UpdateMotor(startingPosition - transform.position);
            } 

        }
        else 
        {

            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {

            if (hits[i] == null) {
                continue;
            }

            if(hits[i].tag == "Fighter" && hits[i].name == "Player") {

                collidingWithPlayer = true;
            }

            hits[i] = null;
        }

    }
    protected override void Death() {

        isDead = true;
        //put death animation
        
        //GameManager.instance.GrantXp(xpValue);
        GameManager.instance.experience++;
        GameManager.instance.ShowText("+" + xpValue + "xp", 30, Color.green, transform.position, Vector3.up * 40, 1.0f);
        Destroy(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg) {

            //put receive damage sound and animation trigger

            if(Time.time - lastImmune > immuneTime) {

                
                lastImmune = Time.time;
                hitpoint -= dmg.damageAmount;
                pushDirection = new Vector3(0.0f, 0.0f, 0.0f) ;

                
                
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, new Color(0.8f,0f, 0.6f),
                transform.position + new Vector3(2f, 0f , 0f), Vector3.up * 25, 0.3f);

                if(hitpoint <= 0) {

                    hitpoint = 0;
                    Death();
                

                }
            }
    }
}
