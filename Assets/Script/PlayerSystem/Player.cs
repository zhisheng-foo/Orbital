using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Mover 
{
    private SpriteRenderer spriteRenderer;
    private int buffer;
    public Vector3 desiredPosition;
    public Vector3 desiredPositionLobby;
    public Vector3 desiredfirstBoss;
    public Vector3 desiredPositionHalloween;
    public Vector3 desiredSecondBoss;
    public Vector3 desiredPositionWinter;
    private string WALK_ANIMATION = "Walk";
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;
    private Vector3 facingDirection = Vector3.right; 
    public Animator anim;
    private bool dodge;
    public Weapon weapon;
    public bool isInvulnerable = false;
    private Rigidbody2D rb2D;
    private bool healMessageShown = false;
    public bool noStackingAtk = false;
    public bool noStackingplot = false;
    private GameObject dustObject;
    public bool isDead;
    /*
    public bool atkbuffed = false;
    incase we need to make it so that the attack dmg of the weapon gets reset every level,
    maybe can adjust at the portal and check if this bool is true + current scene name. 
    */

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;     
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        dustObject = GameObject.Find("Dust");
        SpriteRenderer spriteRendererDust = dustObject.GetComponent<SpriteRenderer>();
        if (scene.name == "Level 3 - 0")
        {
            if (spriteRendererDust != null)
            {
                spriteRendererDust.enabled = true;
            }
        } 
        else 
        {
            spriteRendererDust.enabled = false;
        } 
    
        if (scene.name == "Start Game" ||scene.name == "Victory 1 - 0")
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        if (scene.name == "Level 1 - 0")
        {   
            transform.position = desiredPosition;      
        }

        if (scene.name == "Main")
        {
            transform.position = desiredPositionLobby;
        }

        if (scene.name == "Level 1 - 1")
        {
            transform.position = desiredfirstBoss;
        }

        if (scene.name == "Level 2 - 0") 
        {
            transform.position = desiredPositionHalloween;
        }

        if (scene.name == "Level 2 - 1")
        {
            transform.position = desiredSecondBoss;
        }

        if (scene.name == "Level 3 - 0")
        {
            transform.position = desiredPositionWinter;    
        }
    }

    protected override void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        
        rb2D.freezeRotation = true; 
        SceneManager.sceneLoaded += OnSceneLoaded;
            
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Start Game" 
        || SceneManager.GetActiveScene().name != "Victory 1 - 0")
        {
            if (rb2D != null)
                rb2D.simulated = true;

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            anim.SetBool(WALK_ANIMATION, true);

            if (x > 0 || y > 0)
            {
                anim.SetBool(WALK_ANIMATION, true);
            }
            else if (x < 0 || y < 0)
            {
                anim.SetBool(WALK_ANIMATION, true);
            }
            else
            {
                anim.SetBool(WALK_ANIMATION, false);
            }

            dodge = false;

            UpdateMotor(new Vector3(x, y, 0));
        }

        if (Input.GetKeyDown(KeyCode.V) && SceneManager.GetActiveScene().name == "Level 3 - 0")
        {
            ShootProjectile();
        }

        if ( hitpoint == 0)
        {
            if (!isDead)
            {
                isDead = true;
                Debug.Log("Game over for player");
                FindObjectOfType<GameOverManager>().GameOver();
            }
        }
    }

    public void ReceiveDamage(Damage dmg) 
    {
        if (Time.time - lastImmune > immuneTime) 
        {
            System.Random rnd = new System.Random();

            lastImmune = Time.time;
            pushDirection = Vector3.zero;
            
            if (rnd.Next(2) == 0) 
            {   
                if (!isInvulnerable) 
                {   
                    hitpoint -= dmg.damageAmount;
                    
                    GameManager.instance.ShowText(dmg.damageAmount.ToString(), 20, 
                    new Color(0.3f, 0f, 0.1f), transform.position + new Vector3(2f, 0f, 0f), 
                    Vector3.up * 25, 0.3f);
                }
                else 
                {
                    GameManager.instance.ShowText("Plot Armour", 20, new Color(0.1f, 0.1f, 0.1f),
                        transform.position + new Vector3(1.5f, 0f, 0f), Vector3.up * 25, 0.3f);
                }     
            }
            else 
            {
                dodge = true;
            
                GameManager.instance.ShowText("DODGE", 20, new Color(0.3f, 0f, 0.1f),
                    transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 25, 0.3f);       
            }

            if (hitpoint <= 0) 
            {
                hitpoint = 0;
            }
        }
    }

    protected override void Death()
    {
    }

    public void Heal (int healingAmount) {
        if (hitpoint == maxHitpoint) {
            return;
        }
        hitpoint += healingAmount;
        if (hitpoint > maxHitpoint) {
            hitpoint = maxHitpoint;
        }

        GameManager.instance.ShowText("The chosen grants you + " + healingAmount.ToString() + " hp", 20, 
        Color.black, transform.position, Vector3.up * 30, 1.0f);    
    }

    public IEnumerator ResetPlayerStats(float duration, int originalDamagePoint)
    {
        yield return new WaitForSeconds(duration);
    
        // Reset player's speed and attack to original values
        ySpeed = 5.0f;
        xSpeed = 5.5f;
        weapon.damagePoint = originalDamagePoint;
        this.noStackingAtk = false;
        GameManager.instance.ShowText("Ham Breathing deactivated"
        , 20,new Color(0f, 0f, 0f), transform.position, Vector3.up * 0.45f, 1.0f);
        
    }

    public IEnumerator ResetPlayerStatsTrade(float duration)
    {   
        yield return new WaitForSeconds(duration);

        weapon.damagePoint -= 999;
        GameManager.instance.ShowText("  The curse has been lifted"
        , 20, new Color(0f, 0f, 0f), transform.position + Vector3.up * 0.30f, Vector3.up * 0.65f, 1.0f);
    }

    private void ShootProjectile()
    {
        GameObject projectile 
        = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();

        Vector3 direction = facingDirection.normalized;

        projectileMovement.Initialize(direction, projectileSpeed, projectileLifetime);
    }

    protected override void UpdateMotor(Vector3 input)
    {
        base.UpdateMotor(input);

        if (input != Vector3.zero)
        {
            facingDirection = input;
        }
    }
}



   




