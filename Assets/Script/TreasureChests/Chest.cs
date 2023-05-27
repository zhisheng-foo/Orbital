using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    private Animator anim;
    private string CLOSE_ANIMATION = "Close";
    public int dollarAmount = 10;

    [SerializeField]
    private AudioSource scaredSoundEffect;
    

    protected override void OnCollect() 
    {   
        anim = GetComponent<Animator>();

        if (!collected)
        
        {
            collected = true;
            anim.SetBool(CLOSE_ANIMATION, true);
            scaredSoundEffect.Play();
            GameManager.instance.dollar += dollarAmount;
            GameManager.instance.ShowText("+" + dollarAmount + " TREATS ", 15,
            new Color(0.6f, 0.0f, 0.2f), transform.position,Vector3.up * 25, 2.0f);
        } 
    }
}
