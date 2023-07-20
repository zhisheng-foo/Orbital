using System.Collections;
using UnityEngine;

//This class handles the Boss3_entity
public class Boss3 : Enemy
{
    public GameObject missilePrefab;
    public float missileSpeed = 5f;
    public float missileLifetime = 3f;
    public float missileRotationSpeed = 200f;
    private string PROJECTILE_ANIMATION = "Project";
    private string IDLE_STATE = "boss_idle";
    private string INTRO_ANIMATION_1 = "boss_reverse_death";
    private string INTRO_ANIMATION_2 = "show_off";
    private string TELEPORT_ANIMATION_before = "Teleport";
    private string TELEPORT_ANIMATION_after = "Teleport1";
    private string DEATH_ANIMATION = "Death";
    private Animator animator;
    public float missileSpawnDelay = 1f;
    private AudioSource audioSource;
    public AudioClip projectileAudioClip;
    private bool isPlayingDeathSound = false;
    private float teleportInterval = 3f;
    private float aoeDamageRadius = 0.92f;
    private float aoeDamageAmount = 2f;
    private Vector3 lastPlayerPosition;
    public bool isDead = false;
    private AudioSource audioSource2;
    public AudioClip teleportAudioClip;
    public AudioClip deathAudioClip;
    public bool isInvulnerable = false;



    protected override void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>(); 
        audioSource = GetComponentInChildren<AudioSource>(); 
        audioSource2 = GetComponentInChildren<AudioSource>();

        StartCoroutine(PlayIntroAnimations());
        StartCoroutine(TeleportAndAOEDamage());
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
    }

    private IEnumerator PlayIntroAnimations()
    {
        
        animator.Play(INTRO_ANIMATION_1);
    
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    
        animator.Play(INTRO_ANIMATION_2);
     
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.Play(IDLE_STATE);

        StartCoroutine(ShootMissiles());
    }

    //This method instantiates the homing missiles
    private IEnumerator ShootMissiles()
    {
        while (true)
        {
            ShootMissile();
            yield return new WaitForSeconds(missileSpawnDelay);
        }
    }

    private void ShootMissile()
    {
        Debug.Log("Shoot missile");
        animator.SetBool(PROJECTILE_ANIMATION, true); 

        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        missile.GetComponent<HomingMissile>().Initialize(playerTransform, missileSpeed, missileLifetime);
        Destroy(missile, missileLifetime);

        
        audioSource.PlayOneShot(projectileAudioClip);

        StartCoroutine(ResetAnimation());
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.8f); 
        animator.SetBool(PROJECTILE_ANIMATION, false); 
    }

    protected override void Death()
    {
        if (isPlayingDeathSound)
            return;

        base.anim.SetBool(DEATH_ANIMATION, true);
        isPlayingDeathSound = true;
        base.Death();
        isDead = true;
        float delay = 2.2f;

        StartCoroutine(DestroyWithDelay(delay));
    }


    

    private IEnumerator DestroyWithDelay(float delay)
    {
        

        float delay1 = 0.5f;
        yield return new WaitForSeconds(delay1);
        GameManager.instance.dollar += 50;
        int temp = 50;
        GameManager.instance.ShowText("BEGONE PUMPKIN + " + 50 + " TREATS", 30, new Color(0f, 0f, 0f),
            transform.position, Vector3.up * 20, 1.0f);

        yield return new WaitForSeconds(delay);
     
        DestroyProjectiles();
        Destroy(gameObject);
    }

    private void DestroyProjectiles()
    {
        HomingMissile[] missiles = FindObjectsOfType<HomingMissile>();
        foreach (HomingMissile missile in missiles)
        {
            Destroy(missile.gameObject);
        }
    }

    //This method teleports the Boss 3 to the last updated player position
    private IEnumerator TeleportAndAOEDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(teleportInterval);
            TeleportToPlayer();
            
        }
    }

    private bool isTeleporting = false;

    private void TeleportToPlayer()
    {   
        if (this.hitpoint == 0)
        {
            Death();
        }
        if (playerTransform != null && !isTeleporting)
        {
            lastPlayerPosition = playerTransform.position;
            StartCoroutine(DelayedTeleport());
        }

        
    }

        private IEnumerator DelayedTeleport()
    {
        isTeleporting = true; 

        animator.SetBool(TELEPORT_ANIMATION_before, true);

        yield return new WaitForSeconds(0.6f);

        animator.SetBool(TELEPORT_ANIMATION_after, true);
        audioSource2.PlayOneShot(teleportAudioClip);

        if (playerTransform != null)
        {   
            transform.position = lastPlayerPosition;
            PerformAOEDamage();
        }

        animator.SetBool(TELEPORT_ANIMATION_before, false);
        
        yield return new WaitForSeconds(1f);

        
        animator.SetBool(TELEPORT_ANIMATION_after, false);

        isTeleporting = false;
    }


    //This method performs the leech effect when the Boss collides with the Player
    private void PerformAOEDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(lastPlayerPosition, aoeDamageRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Player" && !this.isDead)
            {
                Player player = collider.GetComponent<Player>();
                if (player != null && player.isInvulnerable == false)
                {
                    player.hitpoint -= 1;
                    this.hitpoint += 2;

                    if (this.hitpoint > this.maxHitpoint)
                    {
                        this.hitpoint = this.maxHitpoint;
                    }

                    StartCoroutine(ShowTextWithDelay("LEECH + 2HP", 40, new Color(0.5f, 0f, 0f), 1.0f));
                } else 

                {
                    StartCoroutine(ShowTextWithDelay("NARUHOTO PLOT ARMOUR", 40, new Color(0.5f, 0f, 0f), 1.0f));
                }
            }
        }
    }


    private IEnumerator ShowTextWithDelay(string text, int fontSize, Color color, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.instance.ShowText(
            text,
            fontSize,
            color,
            transform.position + new Vector3(2.5f, 0f, 0f),
            Vector3.up * 40,
            0.3f
        );
    }
}
