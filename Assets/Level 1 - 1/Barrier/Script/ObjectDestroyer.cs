using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject bossObject; // Reference to the boss object
    private string BURN_ANIMATION = "Burn";
    private Animator anim; // Reference to the Animator component

    private void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component
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
        // Play the burn animation
        anim.SetBool(BURN_ANIMATION, true);

        // Wait for the animation to complete
        float burnAnimationDuration = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(burnAnimationDuration);

        // Destroy this object
        Destroy(gameObject);
    }
}
