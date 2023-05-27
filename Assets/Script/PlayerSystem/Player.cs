using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Mover 
{
    private SpriteRenderer spriteRenderer;

    private int buffer;

    private string WALK_ANIMATION = "Walk";
    private string DODGE_ANIMATION = "Dodge";
    private string ATTACK_ANIMATION = "Attack";

    public Animator anim;
    private bool dodge;

    public Weapon weapon;

    private Rigidbody2D rb2D;

    protected override void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        // Check if the current scene is the main scene
        if (SceneManager.GetActiveScene().name != "Main")
        {
            // Disable Rigidbody2D component
            if (rb2D != null)
                rb2D.simulated = false;

            return;
        }

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
       




