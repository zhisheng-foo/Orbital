using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Manager : MonoBehaviour
{
    public Boss3 boss;
    public GameObject object1Prefab;
    public GameObject object2Prefab;
    public List<Vector3> object1Positions;
    public List<Vector3> object2Positions;
    private GameObject object1;
    private GameObject object2;
    private bool firstSpawned = false;
    private bool secondSpawned = false;
    private bool thirdSpawned = false;
    public AudioClip objectSound;
    private AudioSource audioSource;
    private Canvas canvasInstance;
    public Canvas canvasPrefab;
    public bool isDestroyed = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ((float)boss.hitpoint / (float)boss.maxHitpoint <= 0.75f && !firstSpawned)
        {   
            int randomIndex1 = Random.Range(0, object1Positions.Count);
            int randomIndex2 = Random.Range(0, object2Positions.Count);

            Vector3 position1 = object1Positions[randomIndex1];
            Vector3 position2 = object2Positions[randomIndex2];

            object1 = Instantiate(object1Prefab, position1, Quaternion.identity);
            object2 = Instantiate(object2Prefab, position2, Quaternion.identity);
            canvasInstance = Instantiate(canvasPrefab, transform.position, Quaternion.identity);
            // Play the object sound
            audioSource.PlayOneShot(objectSound);

            firstSpawned = true;
        }

        if ((float)boss.hitpoint / (float)boss.maxHitpoint <= 0.50f && !secondSpawned)
        {   
            int randomIndex1 = Random.Range(0, object1Positions.Count);
            int randomIndex2 = Random.Range(0, object2Positions.Count);

            Vector3 position1 = object1Positions[randomIndex1];
            Vector3 position2 = object2Positions[randomIndex2];

            object1 = Instantiate(object1Prefab, position1, Quaternion.identity);
            object2 = Instantiate(object2Prefab, position2, Quaternion.identity);
            canvasInstance = Instantiate(canvasPrefab, transform.position, Quaternion.identity);
            // Play the object sound
            audioSource.PlayOneShot(objectSound);

            secondSpawned = true;
        }

        if ((float)boss.hitpoint / (float)boss.maxHitpoint <= 0.25f && !thirdSpawned)
        {   
            int randomIndex1 = Random.Range(0, object1Positions.Count);
            int randomIndex2 = Random.Range(0, object2Positions.Count);

            Vector3 position1 = object1Positions[randomIndex1];
            Vector3 position2 = object2Positions[randomIndex2];

            object1 = Instantiate(object1Prefab, position1, Quaternion.identity);
            object2 = Instantiate(object2Prefab, position2, Quaternion.identity);
            canvasInstance = Instantiate(canvasPrefab, transform.position, Quaternion.identity);
            
            audioSource.PlayOneShot(objectSound);

            thirdSpawned = true;
        }

        if(boss.hitpoint <= 0)
        {   
            isDestroyed = true;
            StartCoroutine(DelayedDestroy(3.0f));
        }
    }

    private IEnumerator DelayedDestroy(float delay) {
        {
            yield return new WaitForSeconds(delay);
            
            Destroy(gameObject);
        }
    }
}


