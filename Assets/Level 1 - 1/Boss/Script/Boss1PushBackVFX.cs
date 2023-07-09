using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1PushBackVFX : MonoBehaviour
{

    private Animator VFX;
    public Boss1 boss1;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        VFX = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boss1.isPush)
        {
            VFX.SetTrigger("Pushin");

            audioSource.Play();
        }
    }
}
