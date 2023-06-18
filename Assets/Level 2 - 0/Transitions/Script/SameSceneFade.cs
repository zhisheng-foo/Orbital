using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameSceneFade : Collidable
{
    public Vector3 targetPosition;

    public AudioSource audioSource;
    public float collisionAudioCooldown = 2f; // Cooldown duration in seconds

    private bool canPlayCollisionAudio = true;

    private GameObject player;
    private GameObject fadeObject;

    private string TRANSITION = "work";
    private string SAME_SCENE = "same";

    private Animator fadeAnimator;

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        fadeObject = GameObject.Find("Fade");

        if (fadeObject != null)
        {
            fadeAnimator = fadeObject.GetComponent<Animator>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnCollide(Collider2D coll)
    {   
        if(coll.name != "Player")
        {
            return;
        }
        if (player != null)
        {
            if (fadeAnimator != null)
            {
                fadeAnimator.SetBool(TRANSITION, true);

                if (canPlayCollisionAudio)
                {
                    PlayCollisionAudio();
                    canPlayCollisionAudio = false;
                    StartCoroutine(ResetCollisionAudioCooldown());
                }

                StartCoroutine(WaitForFadeComplete());
            }
            else
            {
                TeleportPlayer();
            }
        }
    }

    private IEnumerator WaitForFadeComplete()
    {
        yield return new WaitForSeconds(1f); // Wait for the first animation to complete

        // Disable the first animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetBool(TRANSITION, false);
            TeleportPlayer();
        }

        // Play the black fade to normal animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetBool(SAME_SCENE, true);
        }

        yield return new WaitForSeconds(1f); // Wait for the black fade animation to complete
    }

    private void TeleportPlayer()
    {
        player.transform.position = targetPosition;
    }

    private void PlayCollisionAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator ResetCollisionAudioCooldown()
    {
        yield return new WaitForSeconds(collisionAudioCooldown);
        canPlayCollisionAudio = true;
    }
}
