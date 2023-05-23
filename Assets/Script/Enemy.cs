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

    private Animator anim;
    private string DEATH_ANIMATION = "Death";
    private string HURT_ANIMATION = "Hurt";

    private string ATTACK_ANIMATION = "Attack";

    [SerializeField]
    public AudioSource hurtSoundEffect;

    [SerializeField] 
    private AudioClip receiveDamageSound;

    [SerializeField] 
    private float receiveDamageVolume = 1.0f;



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

        isPlayingDeathSound = true;
        anim.SetBool(DEATH_ANIMATION,true);
        StartCoroutine(DestroyAfterAnimation());
        
        //GameManager.instance.GrantXp(xpValue);
     
    }

    private IEnumerator DestroyAfterAnimation()
    {   
       
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        deathSoundEffect.Play();
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);
        GameManager.instance.experience++;
        GameManager.instance.ShowText("+" + xpValue + "xp", 30, Color.green, transform.position, Vector3.up * 40, 1.0f);
        
        Destroy(gameObject);

    }

    protected override void ReceiveDamage(Damage dmg)
    {
    // Put receive damage sound and animation trigger
       if (!isPlayingReceiveDamageSound && !isPlayingDeathSound & Time.time - lastShout > cooldown2)
        {
            StartCoroutine(PlayReceiveDamageSound());
        }

        if (Time.time - lastImmune > immuneTime)
        {   
            anim.SetBool(HURT_ANIMATION, true);
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = new Vector3(0.0f, 0.0f, 0.0f);

            GameManager.instance.ShowText(
                dmg.damageAmount.ToString(),
                25,
                new Color(0.8f, 0f, 0.6f),
                transform.position + new Vector3(2f, 0f, 0f),
                Vector3.up * 25,
                0.3f
            );

            StartCoroutine(HurtAttackLoop());
            
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

        // Play the receive damage sound
        hurtSoundEffect.PlayOneShot(receiveDamageSound, receiveDamageVolume);

        yield return new WaitForSeconds(receiveDamageSound.length);

        isPlayingReceiveDamageSound = false;
    }   

    private IEnumerator HurtAttackLoop()
    {
        // Wait for the hurt animation to finish playing
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Transition to the attack animation
        anim.SetBool(HURT_ANIMATION, false);
        anim.SetBool(ATTACK_ANIMATION, true);
      
        
        // Wait for the attack animation to finish playing
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Transition back to the hurt animation
        anim.SetBool(ATTACK_ANIMATION, false);
        anim.SetBool(HURT_ANIMATION, true);

        
        // Restart the loop
        StartCoroutine(HurtAttackLoop());
    }
}

    
