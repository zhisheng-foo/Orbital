using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{
    public float[] fireballSpeed = { 2.5f, -2.5f };
    public Transform[] fireballs;

    public int numFireballs = 2;

    public float[] distance;
    public float minDistance = 10f; // Minimum distance to keep from the player
    public float moveSpeed = 5f; // Speed at which the boss moves towards/away from the player

    public Vector2 bossBoundsMin; // Minimum boundary position
    public Vector2 bossBoundsMax; // Maximum boundary position

    public float chaseYPosition = 20f; // Y position at which boss starts chasing

    private string DEATH_ANIMATION = "Death";

    private string ATTACK_ANIMATION = "Attack";
    private bool isPlayingDeathSound = false;

    private Vector3 startPosition; // Initial position of the boss
    private bool shouldChase = false; // Flag to determine if the boss should start chasing

    private bool movingBack = false;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveFireballs();

        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        // Calculate the direction to move towards the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Check if the player has crossed the chaseYPosition to trigger the chase
        if (!shouldChase && playerTransform.position.y >= chaseYPosition)
        {
            shouldChase = true;
        }

        // Move away from the player if the distance is less than the minimum distance
        if (shouldChase && playerDistance < minDistance)
        {
            // Move in the opposite direction of the player
            Vector3 directionAwayFromPlayer = -directionToPlayer;

            // Check if the boss is already moving back
            if (!movingBack)
            {
                // Start moving back
                movingBack = true;
                StartCoroutine(MoveBack());
            }

            UpdateMotor(directionAwayFromPlayer);
        }
        else if (shouldChase)
        {
            // Move towards the player if the distance is greater than the minimum distance
            movingBack = false; // Reset the moving back flag
            UpdateMotor(directionToPlayer);
        }

        // Face left or right based on player movement
        if (playerTransform.position.x < transform.position.x)
        {
            // Player is on the left side, so face left
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            // Player is on the right side, so face right
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Restrict boss movement within the designated area
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bossBoundsMin.x, bossBoundsMax.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bossBoundsMin.y, bossBoundsMax.y);
        transform.position = clampedPosition;
    }

    private IEnumerator MoveBack()
    {
        // Calculate the direction to move back towards the initial position
        Vector3 directionToInitial = (startPosition - transform.position).normalized;

        while (movingBack && transform.position != startPosition)
        {
            UpdateMotor(directionToInitial);

            // Face left or right based on player movement
            if (playerTransform.position.x < transform.position.x)
            {
                // Player is on the left side, so face left
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (playerTransform.position.x > transform.position.x)
            {
                // Player is on the right side, so face right
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            yield return null;
        }
    }

    private void MoveFireballs()
{
    for (int i = 0; i < numFireballs; i++)
    {
        int index = i % fireballSpeed.Length; // Use modulo operator to loop through the fireballSpeed array

        // Calculate the time factor based on fireballSpeed
        float timeFactor = Time.time * fireballSpeed[index];

        // Calculate the circular movement around the boss
        float angle = timeFactor * Mathf.Rad2Deg; // Convert timeFactor to degrees
        Vector3 offset = Quaternion.Euler(0f, 0f, angle) * Vector3.up * distance[index] * 15f;

        // Set the new position relative to the boss
        fireballs[i].position = transform.position + offset;
    }
}


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Death()
    {
        // Play death animation here
        base.anim.SetBool(DEATH_ANIMATION, true);
        isPlayingDeathSound = true;
        // Delayed destruction
        float delay = 0.4f; // Adjust the delay time as needed

        StartCoroutine(DestroyWithDelay(delay));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        deathSoundEffect.Play();
        // Destroy fireball objects
        foreach (Transform fireball in fireballs)
        {
            Destroy(fireball.gameObject);
        }

        float delay1 = 0.5f;
        yield return new WaitForSeconds(delay1);
        GameManager.instance.dollar += 20;
        int temp = 20;
        GameManager.instance.ShowText("YEEE BOII + " + temp + " TREATS", 30 , new Color(1f, 1f, 0.5f),
        transform.position, Vector3.up * 20, 1.0f);

        yield return new WaitForSeconds(delay);

        // Destroy the boss object
        Destroy(gameObject);
    }
}
