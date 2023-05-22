using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 3.0f;

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    private float cooldown = 0.1f;
    private float lastswing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastswing = Time.time;
            Swing();
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {   
           if (coll.name == "Player")
           {
                return;
           }

           Damage dmg = new Damage()
           {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce

           };
           

           coll.SendMessage("ReceiveDamage", dmg);
           
        }
       
    }

    private void Swing()
    {   
        //put attack animation of Dogo here
        Debug.Log("Swing");
    }    
}
