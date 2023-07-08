using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobSpawner : MonoBehaviour
{
    public GameObject[] mobPrefabs;
    public int[] numMobsToSpawn;


    public Boss3 boss;

    private List<GameObject> mobList = new List<GameObject>();

    private int totalNumMobs = 0;
    private int numMobsKilled = 0;

    private string DEATH_ANIMATION = "Death";

    private bool isDestroyed = false;


    private void Start()
    {
        SpawnMobs();
        
    }

    private IEnumerator SpawnMobsWithDelay(float initialDelay, float delay)
    {   
        
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < mobPrefabs.Length; i++)
        {
            for (int j = 0; j < numMobsToSpawn[i]; j++)
            {
                GameObject mob = Instantiate(mobPrefabs[i], transform.position, Quaternion.identity);
                mobList.Add(mob);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private void SpawnMobs()
    {
        for (int i = 0; i < mobPrefabs.Length; i++)
        {
            totalNumMobs += numMobsToSpawn[i];
        }
        Debug.Log(totalNumMobs + " mobs to spawn");

        float initialDelay = 4.0f;
        float delay = 1.0f;

        StartCoroutine(SpawnMobsWithDelay(initialDelay, delay));
    }

    private void Update()
    {   
        foreach (GameObject mob in mobList)
        {
            Enemy enemy = mob.GetComponent<Enemy>();

            if (enemy.hitpoint == 0)
            {
                numMobsKilled++;
                mobList.Remove(mob);
                break;
            }
        }

        Debug.Log(numMobsKilled + " mobs killed");

        if (numMobsKilled == totalNumMobs)
        {
            float delay = 2.0f; // Set the delay value (in seconds) here

            StartCoroutine(DestroyWithDelay(delay));
        }
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Animator anim = GetComponent<Animator>();
        anim.SetBool(DEATH_ANIMATION, true);

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        isDestroyed = true;
        Destroy(gameObject);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}
