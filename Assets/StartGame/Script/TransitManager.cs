using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitManager : MonoBehaviour
{
    public Animator transitionAnim;
    void Start() {

        transitionAnim.SetBool("work", false);

    }  
}
