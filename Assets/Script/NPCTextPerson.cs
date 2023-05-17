using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
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
        GameManager.instance.ShowText(message, 35 , Color.black, transform.position ,Vector3.zero, cooldown);
    }
   }
}
