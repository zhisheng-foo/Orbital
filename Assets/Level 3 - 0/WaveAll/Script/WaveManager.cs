using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the sequence for all waves 
//it follows a destroy - than instantiate approach
public class WaveManager : MonoBehaviour
{
    public GameObject firstObjectPrefab;
    public GameObject secondObjectPrefab;
    public GameObject thirdObject;
    private GameObject firstObjectInstance;
    private GameObject secondObjectInstance;
    private GameObject thirdObjectInstance;
    public bool isFirstObjectDestroyed = false;
    public bool isSecondObjectDestroyed = false;
    private int counter = 0;
    private int counter1 = 0;

    void Start()
    {
        firstObjectInstance = Instantiate(firstObjectPrefab);
    }
    
    void Update()
    {
        if (firstObjectInstance == null && counter != 1)
        {
            isFirstObjectDestroyed = true;
            secondObjectInstance = Instantiate(secondObjectPrefab);
            counter++;
        }

        if (isFirstObjectDestroyed && !isSecondObjectDestroyed)
        {
            if (secondObjectInstance == null)
            {
                isSecondObjectDestroyed = true;
                thirdObjectInstance = Instantiate(thirdObject);
            }
        }
    }
}
