using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Inherits from collidables and ensures that only the player class can interact with the collectable
*/

public class Collectable : Collidable
{
    protected bool collected;
    protected override void OnCollide(Collider2D coll) 
    {
        
        if(coll.name == "Player")
        {
            OnCollect();
        }
    }
    protected virtual void OnCollect() 
    {
        collected = true;
    } 
}
