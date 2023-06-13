using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject barrierPrefab; // The barrier prefab to instantiate
    public GameObject bossObject; // Reference to the boss object
    public float delay = 0.01f; // Delay in seconds before instantiating the barrier
    public AudioSource audioSource; // Reference to the AudioSource component to play the sound

    private bool barrierSpawned = false; // Flag to track if the barrier has been spawned
    private int counter = 0;
    private Transform playerTransform; // Reference to the player's transform component

    private void Start()
    {
        // Find the player object by name
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found with the name 'Player'");
        }
    }

    private void Update()
    {
        // Check if the playerTransform exists and if the player's position is above the barrier's position
        if (playerTransform != null && playerTransform.position.y > transform.position.y && !barrierSpawned)
        {
            StartCoroutine(DelayedSpawnBarrier());
        }

        // Check if the boss object is destroyed
        if (bossObject == null)
        {
            DestroyBarrier();
        }
    }

    private IEnumerator DelayedSpawnBarrier()
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the barrier object
        if (counter != 1)
        {
            GameObject barrier = Instantiate(barrierPrefab, transform.position, Quaternion.identity);
            barrier.transform.SetParent(transform); // Set the barrier as a child of the BarrierController game object
            barrierSpawned = true;
            counter++;

            // Play the barrier sound
            audioSource.Play();
        }
    }

    private void DestroyBarrier()
    {
        Destroy(gameObject);
    }
}
