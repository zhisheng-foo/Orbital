using System.Collections;
using UnityEngine;

public class Boss3 : Enemy
{
    public GameObject missilePrefab;
    public float missileSpeed = 5f;
    public float missileLifetime = 3f;
    public float missileRotationSpeed = 200f;
    public string PROJECTILE_ANIMATION = "Project";
    public string IDLE_STATE = "boss_idle";
    public string INTRO_ANIMATION_1 = "boss_reverse_death";
    public string INTRO_ANIMATION_2 = "show_off";

    private Animator animator;
    public float missileSpawnDelay = 1f;

    private AudioSource audioSource;
    public AudioClip projectileAudioClip;

    protected override void Start()
    {
        playerTransform = GameObject.Find("Player").transform; // Assign player's transform
        animator = GetComponent<Animator>(); // Assign the Animator component
        audioSource = GetComponentInChildren<AudioSource>(); // Assign the AudioSource component from a child object
        
        StartCoroutine(PlayIntroAnimations());
    }

    private IEnumerator PlayIntroAnimations()
    {
        // Play the first introduction animation
        animator.Play(INTRO_ANIMATION_1);
        
        // Wait for the duration of the first introduction animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        // Play the second introduction animation
        animator.Play(INTRO_ANIMATION_2);
        
        // Wait for the duration of the second introduction animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        // Transition to the idle state
        animator.Play(IDLE_STATE);
        
        StartCoroutine(ShootMissiles());
    }

    private IEnumerator ShootMissiles()
    {
        while (true)
        {
            ShootMissile();
            yield return new WaitForSeconds(missileSpawnDelay);
        }
    }

    private void ShootMissile()
    {
        Debug.Log("Shoot missile");
        animator.SetBool(PROJECTILE_ANIMATION, true); // Set the boolean parameter to trigger the animation

        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        missile.GetComponent<HomingMissile>().Initialize(playerTransform, missileSpeed, missileLifetime);
        Destroy(missile, missileLifetime);

        // Play the audio clip
        audioSource.PlayOneShot(projectileAudioClip);

        StartCoroutine(ResetAnimation()); // Reset the animation after a delay
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.8f); // Delay before resetting the animation
        animator.SetBool(PROJECTILE_ANIMATION, false); // Set the boolean parameter to trigger the animation
    }
}
