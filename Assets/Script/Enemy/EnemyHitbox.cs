using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Hitbox of the enemy class that is attached to all enemy gameobjects.
Will handle the receving of damage from player or other sources
Inherits from collidable class
*/

public class EnemyHitbox : Collidable
{
    public int damage = 1;
    private float pushForce = 4.0f;
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
