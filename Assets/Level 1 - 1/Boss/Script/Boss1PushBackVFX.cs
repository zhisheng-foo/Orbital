using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Handles the push force logic of Boss1
public class Boss1PushBackVFX : MonoBehaviour
{
    private Animator VFX;
    public Boss1 boss1;
    private AudioSource audioSource;
    void Start()
    {
        VFX = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(boss1.isPush)
        {
            VFX.SetTrigger("Pushin");
            audioSource.Play();
        }
    }
}
