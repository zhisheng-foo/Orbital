using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Animation for the player class
*/

public class BoredBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float timeUntilBored;

    [SerializeField]
    private int numberOfBoredAnimation;
    private bool isBored;
    private float idleTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBored == false)
        {
            idleTime += Time.deltaTime;

            if (idleTime > timeUntilBored)
            {
                isBored = true;
                int boredAnimation = Random.Range(1, numberOfBoredAnimation + 1);
                animator.SetFloat("Bored", boredAnimation);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle(animator);
        }     
    }
    
    private void ResetIdle(Animator animator)
    {
        isBored = false;
        idleTime = 0;

        animator.SetFloat("Bored", 0);
    }
}
