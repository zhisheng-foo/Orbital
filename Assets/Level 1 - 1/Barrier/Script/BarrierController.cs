using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject barrierPrefab; 
    public GameObject bossObject; 
    public float delay = 0.01f; 
    public AudioSource audioSource; 
    private bool barrierSpawned = false; 
    private int counter = 0;
    private Transform playerTransform; 
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
        
        if (playerTransform != null 
        && playerTransform.position.y > transform.position.y && !barrierSpawned)
        {
            StartCoroutine(DelayedSpawnBarrier());
        }
   
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
            barrier.transform.SetParent(transform); 
            barrierSpawned = true;
            counter++;

        
            audioSource.Play();
        }
    }

    private void DestroyBarrier()
    {
        Destroy(gameObject);
    }
}
