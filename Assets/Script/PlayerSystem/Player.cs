using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Mover 
{
    private SpriteRenderer spriteRenderer;
    private int buffer;

    public Vector3 desiredPosition;

    private string WALK_ANIMATION = "Walk";
    private string DODGE_ANIMATION = "Dodge";
    private string ATTACK_ANIMATION = "Attack";

    public Animator anim;
    private bool dodge;

    public Weapon weapon;

    private Rigidbody2D rb2D;


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
    rb2D.velocity = Vector2.zero;
    rb2D.angularVelocity = 0f;


    if (scene.name == "Level 1 - 0")
    {   
        transform.position = desiredPosition;
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}



    protected override void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();


        rb2D.freezeRotation = true; // Prevent rotation

        SceneManager.sceneLoaded += OnSceneLoaded;
        
      
    }
  

      private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Start Game")
        {
            // Enable Rigidbody2D component
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
    }


    public void OnLevelUp() 
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level) 
    {
        for (int i = 0; i < level; i++) 
        {
            OnLevelUp();
        }
    }

    protected override void ReceiveDamage(Damage dmg) 
    {
        if (Time.time - lastImmune > immuneTime) 
        {
            System.Random rnd = new System.Random();

            lastImmune = Time.time;
            pushDirection = Vector3.zero;

            if (rnd.Next(2) == 0) 
            {
                hitpoint -= dmg.damageAmount;
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, new Color(0.3f, 0f, 0.1f),
                    transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 25, 0.3f);
            }
            else 
            {
                dodge = true;
                anim.SetBool(DODGE_ANIMATION, true);
                GameManager.instance.ShowText("DODGE", 20, new Color(0.3f, 0f, 0.1f),
                    transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 25, 0.3f);
                StartCoroutine(StopDodgeAnimation());
            }

            if (hitpoint <= 0) 
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    private IEnumerator StopDodgeAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); 

        anim.SetBool(DODGE_ANIMATION, false);
    }
}


   




