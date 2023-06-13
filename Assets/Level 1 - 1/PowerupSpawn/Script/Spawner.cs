using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array of objects to spawn
    public float spawnInterval = 25f; // Interval between spawns in seconds

    private GameObject spawnedObject; // Reference to the currently spawned object

    private void Start()
    {
        // Start spawning objects immediately
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (spawnedObject == null)
            {
                // Spawn a random object from the array
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
            else
            {
                // Destroy the previously spawned object
                Destroy(spawnedObject);

                // Spawn a new random object from the array
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }

            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
