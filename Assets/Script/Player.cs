using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover 
{
     
    private SpriteRenderer spriteRenderer;

    private int buffer;

    private string WALK_ANIMATION = "Walk";

    private Animator anim;

    protected override void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void FixedUpdate() 
    {

     float x = Input.GetAxisRaw("Horizontal");
     float y = Input.GetAxisRaw("Vertical");

     anim.SetBool(WALK_ANIMATION, true);

     if (x > 0 || y > 0) 
       {
            anim.SetBool(WALK_ANIMATION, true);
       }
     else if (x < 0 || y < 0) 
       {
            anim.SetBool(WALK_ANIMATION,true);
       }
     else 
       {
            anim.SetBool(WALK_ANIMATION,false);
       } 

     UpdateMotor(new Vector3(x,y, 0));
    }

    public void OnLevelUp() 
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level) 
    {
        for(int i = 0; i < level; i++) 
        {
            OnLevelUp();
        }
    }

    protected override void ReceiveDamage(Damage dmg) {

        //put receive damage sound and animation trigger
        
        if(Time.time - lastImmune > immuneTime) {
            System.Random rnd = new System.Random();

            buffer++;
            lastImmune = Time.time;
            pushDirection = new Vector3(0.0f, 0.0f, 0.0f) ;

            if (rnd.Next(2) == 0)
            {
              hitpoint -= dmg.damageAmount;  
              GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, new Color(0.3f, 0f, 0.1f),
              transform.position + new Vector3(2f, 0f , 0f), Vector3.up * 25, 0.3f);
            }

            if (hitpoint <= 0) {

                hitpoint = 0;
                Death();
               

            }
        }
    }
    
}
