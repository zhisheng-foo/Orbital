using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson1 : Collidable
{
   public string message;

   private float cooldown = 1.0f;
   private float lastShout;

   protected override void Start() {
    base.Start();
    lastShout = - cooldown;
   }

   protected override void OnCollide(Collider2D coll) {

    if (Time.time - lastShout > cooldown) {
        lastShout = Time.time;
        GameManager.instance.ShowText(message, 25  , new Color(1.0f, 0.1f, 0.3f), transform.position  + new Vector3(0, 0.85f, 0) ,Vector3.zero, cooldown);
    }
   }
}

