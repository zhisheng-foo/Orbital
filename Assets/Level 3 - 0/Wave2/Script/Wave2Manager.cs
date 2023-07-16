using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave2Manager : MonoBehaviour
{
    public GameObject[] firstObjectsPrefab;
    public GameObject[] secondObjectsPrefab;
    public GameObject[] thirdObjectsPrefab;
    private GameObject[] firstObjectInstances;
    private GameObject[] secondObjectInstances;
    private GameObject[] thirdObjectInstances;
    private bool isFirstObjectsDestroyed = false;
    private bool isSecondObjectsDestroyed = false;
    private bool isThirdObjectsDestroyed = false;
    private int counter = 0;
    public Canvas waveTitle;
    public Canvas hardTitle;

    void Start()
    {   
        Instantiate(waveTitle);
        InstantiateFirstObjects();
    }
    
    void Update()
    {
        if (!isSecondObjectsDestroyed && AreObjectsDestroyed(firstObjectInstances))
        {
            isSecondObjectsDestroyed = true;
            InstantiateSecondObjects();
        }

        if (isSecondObjectsDestroyed && AreObjectsDestroyed(secondObjectInstances) && counter != 1)
        {   
            Instantiate(hardTitle);
            isThirdObjectsDestroyed = true;
            InstantiateThirdObjects();
            counter++;
        }

        if (AreObjectsDestroyed(thirdObjectInstances))
        {
            Destroy(gameObject);
        }
    }

    private void InstantiateFirstObjects()
    {
        firstObjectInstances = new GameObject[firstObjectsPrefab.Length];
        for (int i = 0; i < firstObjectsPrefab.Length; i++)
        {
            firstObjectInstances[i] = Instantiate(firstObjectsPrefab[i]);
        }
    }

    private void InstantiateSecondObjects()
    {
        secondObjectInstances = new GameObject[secondObjectsPrefab.Length];
        for (int i = 0; i < secondObjectsPrefab.Length; i++)
        {
            secondObjectInstances[i] = Instantiate(secondObjectsPrefab[i]);
        }
    }

    private void InstantiateThirdObjects()
    {
        thirdObjectInstances = new GameObject[thirdObjectsPrefab.Length];
        for (int i = 0; i < thirdObjectsPrefab.Length; i++)
        {
            thirdObjectInstances[i] = Instantiate(thirdObjectsPrefab[i]);
        }
    }

    private bool AreObjectsDestroyed(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                return false;
            }
        }
        return true;
    }
}
