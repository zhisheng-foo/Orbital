using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; 
    public float spawnInterval = 25f; 
    private GameObject spawnedObject; 
    private void Start()
    {      
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (spawnedObject == null)
            {
                
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
            else
            {
                
                Destroy(spawnedObject);
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
             
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
