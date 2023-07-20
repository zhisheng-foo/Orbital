using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
Basic class for all hostile NPCs that the player will face in the game.
By default will chase the player class when entering a certain range.
Inherits from Mover Class.
*/

public class Enemy : Mover
{ 
    public float triggerLength = 0.3f;
    public float chaseLength = 0.5f;
    public bool isDestroyed = false;
    public bool isDead = false;
    public float pushDistance = 100f;
    private bool chasing;
    private bool collidingWithPlayer;
    public Transform playerTransform;
    private Vector3 startingPosition;
    public Animator anim;
    private string DEATH_ANIMATION = "Death";
    private string HURT_ANIMATION = "Hurt";
    private string ATTACK_ANIMATION = "Attack";

    [SerializeField]
    public AudioSource hurtSoundEffect;

    [SerializeField] 
    private AudioClip receiveDamageSound;

    [SerializeField] 
    public float receiveDamageVolume = 1.0f;

    [SerializeField]
    public AudioSource deathSoundEffect;
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private float cooldown = 0.4f;
    private float cooldown2 = 0.6f;
    private float lastShout;
    private Collider2D[] hits = new Collider2D[10];
    private bool isPlayingReceiveDamageSound = false;
    private bool isPlayingDeathSound = false;
    public event Action OnMobDestroyed;
    public mobSpawner spawner;

    protected override void Start()
    {
        base.Start();
         
        playerTransform = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        anim = GetComponent<Animator>();
        hurtSoundEffect = GetComponent<AudioSource>();
        deathSoundEffect = GetComponent<AudioSource>();
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        lastShout = - cooldown;

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

        anim.SetBool(DEATH_ANIMATION,true);
        StartCoroutine(DestroyAfterAnimation());

    }

    private IEnumerator DestroyAfterAnimation()
    {   
        float delay = 0.5f;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        if (!isPlayingDeathSound)
        {
            isPlayingDeathSound = true;
            deathSoundEffect.Play();
            deathSoundEffect.loop = false;
        }
    
        yield return new WaitForSeconds(delay);
        if (gameObject.name != "Boss" && gameObject.name != "Boss_2" 
        && gameObject.name != "Boss_3")
        {
            GameManager.instance.dollar += 4;
            int temp = 4;
            GameManager.instance.ShowText("+ " + temp + " TREATS", 15 ,
            new Color(1f, 0.0f, 0f), transform.position, Vector3.up * 40, 1.0f);
        }
        if (gameObject.name != "Boss_3" && gameObject.name != "Boss_2")
        {
            Destroy(gameObject);
        }    
    }


    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isPlayingReceiveDamageSound && !isPlayingDeathSound)
        {
            StartCoroutine(PlayReceiveDamageSound());
        }

        if (Time.time - lastImmune > immuneTime)
        {
            anim.SetBool(HURT_ANIMATION, true);
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = new Vector3(0.0f, 0.0f, 0.0f);

            if (gameObject.name == "Boss" || gameObject.name == "Boss_2")
            {
                GameManager.instance.ShowText(
                    dmg.damageAmount.ToString(),
                    70,
                    new Color(0f, 0f, 0f),
                    transform.position + new Vector3(2.5f, 2f, 0f),
                    Vector3.up * 40,
                    0.3f);
            } 
            else if (gameObject.name == "Boss_3")
            {

                 GameManager.instance.ShowText(
                    dmg.damageAmount.ToString(),
                    70,
                    new Color(0f, 0f, 0f),
                    transform.position + new Vector3(2.5f, 0f, 0f),
                    Vector3.up * 40,
                    0.3f);
            }
            else
            {
                GameManager.instance.ShowText(
                    dmg.damageAmount.ToString(),
                    25,
                    new Color(1.0f, 0f, 0f),
                    transform.position + new Vector3(2.5f, 0f, 0f),
                    Vector3.up * 20,
                    0.3f);
            }

            StartCoroutine(HurtAttackLoop());

            ProjectileMovement projectileMovement = GetComponent<ProjectileMovement>();

            if (projectileMovement == null)
            {
                lastImmune = 0f;
            }
            else
            {
                lastImmune = 0;
            }

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }         
        }
    }

    private IEnumerator PlayReceiveDamageSound()
    {
        isPlayingReceiveDamageSound = true;
        if (Time.time - lastImmune > immuneTime)
        {

            hurtSoundEffect.PlayOneShot(receiveDamageSound, receiveDamageVolume);
            yield return new WaitForSeconds(receiveDamageSound.length);
        }
    
        isPlayingReceiveDamageSound = false;
    }   

    private IEnumerator HurtAttackLoop()
    {
        
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
          
        anim.SetBool(HURT_ANIMATION, false);
        anim.SetBool(ATTACK_ANIMATION, true);
      
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        anim.SetBool(ATTACK_ANIMATION, false);
        anim.SetBool(HURT_ANIMATION, true);

        StartCoroutine(HurtAttackLoop());
    }


    public void ApplyForce(Vector2 direction)
    {
        Vector2 pushVector = direction.normalized * pushDistance;
        Vector2 newPosition = (Vector2)transform.position + pushVector;
        transform.position = newPosition;
    }  
}

    
