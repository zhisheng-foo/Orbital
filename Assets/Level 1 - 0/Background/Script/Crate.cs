using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Destroyable objects in scenes.
*/

public class Crate : Collidable
{   
    public AudioSource audioSource;

    private bool hasCollided = false;

    protected override void OnCollide(Collider2D other)
    {  
        if (hasCollided) return; // Ignore subsequent collisions

        audioSource = GetComponent<AudioSource>();   
        if (other.name == "Weapon")
        {
            StartCoroutine(PlaySoundAndDestroy());
            hasCollided = true;
        }
    }

    private IEnumerator PlaySoundAndDestroy()
    {
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        Destroy(gameObject);
    }
}

