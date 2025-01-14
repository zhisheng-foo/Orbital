using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the sequence for Wave 3
//it follows a destroy - than instantiate approach
public class Wave3Manager : MonoBehaviour
{
    public GameObject firstObject;
    public GameObject bossObjectPrefab;
    private GameObject bossObjectInstance;
    private bool isBossDestroyed = false;
    public Canvas waveTitle;
    public Canvas hardTitle;
    private int counter = 0;
    private int counter1 = 0;

    void Start()
    {
        Instantiate(waveTitle);
        Destroy(firstObject);
    }
    
    void Update()
    {   
        Destroy(firstObject);
        if (counter != 1)
        {
            bossObjectInstance = Instantiate(bossObjectPrefab);
            isBossDestroyed = true;
            counter++;
        }

        if (bossObjectInstance == null && isBossDestroyed && counter1 != 1)
        {
            Instantiate(hardTitle);
            counter1++;
            StartCoroutine(DelayedDestroy(3.0f));
        }
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
