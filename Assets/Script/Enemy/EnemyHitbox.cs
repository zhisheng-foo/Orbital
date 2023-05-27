using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //Damage

    public int damage = 1;
    public float pushForce = 4.0f;

    public bool isColliding = false;

    protected override void OnCollide(Collider2D coll)
    {   
        if (coll.name == "Player" & coll.tag == "Fighter")
        {
                Damage dmg = new Damage()
           {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce

           };
                   
           coll.SendMessage("ReceiveDamage", dmg);
           
        }
    }
}
