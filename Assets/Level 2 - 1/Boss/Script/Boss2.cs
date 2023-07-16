using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    public GameObject[] projectilePrefabs;
    public float projectileSpeed = 5f;
    public float fireRate = 2f;
    public float increasedFireRate = 1f; 
    public float restDuration = 1f; 
    public Transform[] firePoints;
    public Vector2[] restPositions;
    public float shootingRange = 25f;
    private float fireTimer;
    private bool alternateDirection;
    private int fireCount; 
    private bool resting;
    private float restTimer;
    private AudioSource audioSource;
    private AudioSource rangeAudioSource;
    private string DEATH_ANIMATION = "Death";
    private string TELEPORT_ANIMATION = "Teleport";
    private string NORMAL_ANIMATION = "Idle";
    private int counter = 0;
    private bool isAnimatingTeleport;
    private float teleportAnimationDuration = 1f;
    private bool isPlayingDeathSound = false;
    private void Start()
    {
        transform.position = new Vector3(88.15f, 98.11f, 0f);
        fireTimer = fireRate;
        alternateDirection = false; 
        fireCount = 0; 
        resting = false; 
        restTimer = 0f; 
        Transform audioChild = transform.Find("Teleport");
        Transform audioChild2 = transform.Find("Range");

        if (audioChild != null)
        {
            audioSource = audioChild.GetComponent<AudioSource>();
        }
        if(audioChild2 != null)
        {
            rangeAudioSource = audioChild2.GetComponent<AudioSource>();
        }

    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {   
            
            if (!resting)
            {
                fireTimer -= Time.deltaTime;

                if (fireTimer <= 0f)
                {
                    DestroyProjectiles(); 
                    if (fireCount % 2 == 0 && fireCount != 0)
                    {
                        resting = true;
                        restTimer = restDuration;
                        Vector2 randomRestPosition = GetRandomRestPosition();
                        StartCoroutine(TeleportAnimation(randomRestPosition));
                    }
                    else
                    {
                        FireProjectiles();
                        fireCount++;

                        if (fireCount % 3 == 0 && fireCount != 0)
                        {
                            FireOtherProjectiles();
                            fireTimer = fireRate - increasedFireRate;
                        }
                        else if (fireCount % 4 == 0 && fireCount != 0)
                        {
                            FireDirectionalProjectiles();
                            fireTimer = fireRate + 2;
                        }
                        else
                        {
                            fireTimer = fireRate;
                        }

                        alternateDirection = !alternateDirection; 
                    }
                }

                if (counter != 1) {
                    rangeAudioSource.Play();
                    counter++;
                }
                
            }
            else
            {
                restTimer -= Time.deltaTime;

                if (restTimer <= 0f)
                {
                    resting = false;
                    fireCount++;
                    fireTimer = fireRate;
                    audioSource.Play();
                }
            }
        }
    }

    private IEnumerator TeleportAnimation(Vector2 targetPosition)
    {
        if (isAnimatingTeleport)
            yield break;

        isAnimatingTeleport = true;

        base.anim.SetBool(TELEPORT_ANIMATION, true);
   
        yield return new WaitForSeconds(teleportAnimationDuration);

        transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
   
        base.anim.SetBool(TELEPORT_ANIMATION, false);
   
        yield return new WaitForSeconds(teleportAnimationDuration);

        base.anim.SetBool(NORMAL_ANIMATION, true);

        yield return new WaitForSeconds(teleportAnimationDuration);

        if (hitpoint == 0) {
            Death();
        }
    
        isAnimatingTeleport = false;
    }

    protected override void Death()
    {   
        if (isPlayingDeathSound)
            return;

        base.anim.SetBool(DEATH_ANIMATION, true);
        isPlayingDeathSound = true;
        base.Death();
        float delay = 0.54f;

        StartCoroutine(DestroyWithDelay(delay)); 
    }

    private IEnumerator DestroyWithDelay(float delay)
    {   
        
        deathSoundEffect.Play();
    
        float delay1 = 0.5f;
        yield return new WaitForSeconds(delay1);
        GameManager.instance.dollar += 30;
        int temp = 30;
        GameManager.instance.ShowText(" SLAP HIM HARD + " + 30 + " TREATS", 35 ,
         new Color(0f, 0f, 0f),
        transform.position, Vector3.up * 20, 1.0f);

        yield return new WaitForSeconds(delay); 
        Destroy(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void FireProjectiles()
    {
        float angleStep = 20f;

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < 18; i++)
            {
                float angle = i * angleStep;

               
                if (alternateDirection)
                {
                    
                    angle = (i * angleStep) + (angleStep / 2f);
                }

                
                Vector3 direction = Quaternion.Euler(0f, 0f, angle) * firePoint.up;

                GameObject projectilePrefab = projectilePrefabs[0]; 
                GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.position.x,
                 transform.position.y, 0f), firePoint.rotation);
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                projectileMove.MoveDirection = direction.normalized;
                projectileMove.MoveSpeed = projectileSpeed+2;

                if (Mathf.Approximately(angle, 90f) || Mathf.Approximately(angle, 270f))
                {
                    projectileMove.SetDestroyDelay(5f);
                }
                else
                {
                    projectileMove.SetDestroyDelay(5f);
                }
            }
        }
    }

    private void FireDirectionalProjectiles()
    {
        float angleStep = 90f;

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = i * angleStep;

                
                Vector3 direction = Quaternion.Euler(0f, 0f, angle) * firePoint.up;

                GameObject projectilePrefab = projectilePrefabs[2]; 
                GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.position.x,
                 transform.position.y, 0f), firePoint.rotation);
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                projectileMove.MoveDirection = direction.normalized;
                projectileMove.MoveSpeed = projectileSpeed + 4;

                if (Mathf.Approximately(angle, 90f) || Mathf.Approximately(angle, 270f))
                {
                    projectileMove.SetDestroyDelay(3f);
                }
                else
                {
                    projectileMove.SetDestroyDelay(3f);
                }
            }
        }
    }

    private void FireOtherProjectiles()
    {
        float angleStep = 45f;

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * angleStep;

                
                if (alternateDirection)
                {
                    
                    angle = (i * angleStep) + (angleStep / 2f);
                }

               
                Vector3 direction = Quaternion.Euler(0f, 0f, angle) * firePoint.up;

                GameObject projectilePrefab = projectilePrefabs[1]; 
                GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.position.x,
                 transform.position.y, 0f), firePoint.rotation);
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                projectileMove.MoveDirection = direction.normalized;
                projectileMove.MoveSpeed = projectileSpeed + 5;

                if (Mathf.Approximately(angle, 90f) || Mathf.Approximately(angle, 270f))
                {
                    projectileMove.SetDestroyDelay(10f);
                }
                else
                {
                    projectileMove.SetDestroyDelay(10f);
                }
            }
        }
    }

    private GameObject GetRandomProjectilePrefab()
    {
        
        int index = Random.Range(1, projectilePrefabs.Length);
        return projectilePrefabs[index];
    }

    private void DestroyProjectiles()
    {
        
        ProjectileMove[] projectiles = FindObjectsOfType<ProjectileMove>();
        foreach (ProjectileMove projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }
    }

    private Vector2 GetRandomRestPosition()
    {
        if (restPositions.Length > 0)
        {
            int randomIndex = Random.Range(0, restPositions.Length);
            return restPositions[randomIndex];
        }
        
        return Vector2.zero;
    }
}
