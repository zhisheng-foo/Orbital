using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Updated class of NPC with no animation
*/
public class NPCTextPerson2 : Collidable
{
    public string message;
    private float cooldown = 0.4f;
    private float cooldown2 = 1.5f;
    private float lastShout;

    [SerializeField]
    private AudioSource voiceSoundEffect;

    protected override void Start() {

        base.Start();
        lastShout = - cooldown;
        
    }
    protected override void OnCollide(Collider2D coll) {

        if (Time.time - lastShout > cooldown2) 
        {
            voiceSoundEffect.Play();
        }

        if (Time.time - lastShout > cooldown) 
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 25 , new Color(0f, 0f, 0f), 
            transform.position + new Vector3(0, 0.40f, 0) ,Vector3.zero, cooldown);
        }
    }
}
