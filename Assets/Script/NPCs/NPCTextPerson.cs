using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
   public string message;

   private float cooldown = 0.4f;
   private float lastShout;

   [SerializeField]
    private AudioSource voiceSoundEffect;

   protected override void Start() {
    base.Start();
    lastShout = - cooldown;
   }

   protected override void OnCollide(Collider2D coll) {

    if (Time.time - lastShout > cooldown) {
        lastShout = Time.time;
        voiceSoundEffect.Play();
        GameManager.instance.ShowText(message, 25 , Color.black, transform.position ,Vector3.zero, cooldown * 1.05f);
    }
   }
}
