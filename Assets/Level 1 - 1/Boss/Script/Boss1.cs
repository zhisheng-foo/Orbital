using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class handles the object with the Boss1 
//fireballs are instantiatedied which allow some defence mechanism 
//for Boss1
public class Boss1 : Enemy
{
    public float[] fireballSpeed = { 2.5f, -2.5f };
    public Transform[] fireballs;

    public int numFireballs = 2;
    public float[] distance;
    public float minDistance = 10f; 
    public float moveSpeed = 2.5f; 
    public Vector2 bossBoundsMin; 
    public Vector2 bossBoundsMax; 
    public float chaseYPosition = 20f; 
    private string DEATH_ANIMATION = "Death";
    private string ATTACK_ANIMATION = "Attack";
    private bool isPlayingDeathSound = false;
    private Vector3 startPosition; 
    private bool shouldChase = false; 
    private bool movingBack = false;
    private float fireballTimer = 0f;
    public float fireballDisableDuration = 7f;
    public float fireballEnableInterval = 10f;
    private Rigidbody2D playerRigidbody;
    public float knockbackForce = 10f;
    public float knockbackInterval = 10f;
    private float knockbackTimer = 0f;
    private Animator BossPushTrig;
    public bool isPush = false;
    private bool isActive = false;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;

        playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {        
        if(!isActive && playerTransform.position.y >= 17f)
        {
            isActive = true;
        }
        
        if(!isActive)
        {
            return; //Exit update loop i guess
        }
        
        
        
        //1st mech(rotating fireballs)
        MoveFireballs();

        //2nd mech(Kiting player)
        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if (!shouldChase && playerTransform.position.y >= chaseYPosition)
        {
            shouldChase = true;
        }

        if (shouldChase && playerDistance < minDistance)
        {
            Vector3 directionAwayFromPlayer = -directionToPlayer;
   
            if (!movingBack)
            {
                movingBack = true;
                StartCoroutine(MoveBack());
            }

            UpdateMotor(directionAwayFromPlayer);
        }
        else if (shouldChase)
        {
            movingBack = false; 
            UpdateMotor(directionToPlayer);
        }
        if (playerTransform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bossBoundsMin.x, bossBoundsMax.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bossBoundsMin.y, bossBoundsMax.y);
        transform.position = clampedPosition;

        //mech 1.5(fireball on and off)
        if (fireballTimer >= fireballEnableInterval)
        {
            DisableFireballs();
            StartCoroutine(EnableFireballsAfterDelay(fireballDisableDuration));
            fireballTimer = 0f;
        }
        else
        {
            fireballTimer += Time.deltaTime;
        }

        //3rd mech (fart)
        if(knockbackTimer >= knockbackInterval)
        {
            Fart((playerTransform.position - transform.position).normalized * 3f);
            Debug.Log("Push");
            knockbackTimer = 0f;
            isPush = true;
        }
        else
        {
            knockbackTimer += Time.deltaTime;
            isPush = false;
        }
    }

    
    private void Fart(Vector2 direction)
    {
        playerRigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator MoveBack()
    {
        
        Vector3 directionToInitial = (startPosition - transform.position).normalized;

        while (movingBack && transform.position != startPosition)
        {
            UpdateMotor(directionToInitial);

            
            if (playerTransform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (playerTransform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            yield return null;
        }
    }

    private void MoveFireballs()
    {
        for (int i = 0; i < numFireballs; i++)
        {
            int index = i % fireballSpeed.Length;

        
            float timeFactor = Time.time * fireballSpeed[index];

        
            float angle = timeFactor * Mathf.Rad2Deg; 
            Vector3 offset = Quaternion.Euler(0f, 0f, angle) 
            * Vector3.up * distance[index] * 15f;
  
            fireballs[i].position = transform.position + offset;
        }
    }

    private void DisableFireballs()
    {
        foreach(Transform fireball in fireballs)
        {
            fireball.gameObject.SetActive(false);
        }
    }

    private IEnumerator EnableFireballsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach(Transform fireball in fireballs)
        {
            fireball.gameObject.SetActive(true);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Death()
    {     
        base.anim.SetBool(DEATH_ANIMATION, true);
        isPlayingDeathSound = true;
        
        float delay = 0.4f; 

        StartCoroutine(DestroyWithDelay(delay));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        deathSoundEffect.Play();
        
        foreach (Transform fireball in fireballs)
        {
            Destroy(fireball.gameObject);
        }

        float delay1 = 0.5f;
        yield return new WaitForSeconds(delay1);
        GameManager.instance.dollar += 20;
        int temp = 20;
        GameManager.instance.ShowText("YEEE BOII + " 
        + temp + " TREATS", 30 , new Color(1f, 1f, 0.5f),
        transform.position, Vector3.up * 20, 1.0f);

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
