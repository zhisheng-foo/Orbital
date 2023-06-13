using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject bossObject; 
    private string BURN_ANIMATION = "Burn";
    private Animator anim; 

    private void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        // Check if the boss object is defeated
        if (bossObject == null)
        {   
            StartCoroutine(DestroyObjectWithDelay());
        }
    }

    private IEnumerator DestroyObjectWithDelay()
    {
 
        anim.SetBool(BURN_ANIMATION, true);

        // Wait for the animation to complete
        float burnAnimationDuration = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(burnAnimationDuration);

        // Destroy this object
        Destroy(gameObject);
    }
}
