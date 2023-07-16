using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMain : MonoBehaviour
{
    public Animator transitionAnim;
    void Start() {

        transitionAnim.SetBool("Transit", false);
        
    }  
}
