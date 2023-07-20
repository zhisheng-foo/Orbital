using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles the homing missile logic
public class HomingMissile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float lifetime;
    private string DEATH_ANIMATION = "Death";
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {   
        Boss3 boss = GameObject.Find("Boss_3").GetComponent<Boss3>();

        if (boss.isDead)
        {
            Destroy(gameObject);
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {   
            Destroy(gameObject);
        }
    }

    public void Initialize(Transform targetTransform, float missileSpeed, float missileLifetime)
    {
        target = targetTransform;
        speed = missileSpeed;
        lifetime = missileLifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
        
            StartCoroutine(DestroyAfterDelay(1.3f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool(DEATH_ANIMATION, true);

        if (audioSource != null)
        {
            
            audioSource.Play();
        }

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
