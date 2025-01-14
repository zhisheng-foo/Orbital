using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the sequence for Wave 1 
//it follows a destroy - than instantiate approach
public class Wave1Manager : MonoBehaviour
{
    public GameObject firstObjectPrefab;
    public GameObject secondObjectPrefab;
    public GameObject thirdObject;
    public GameObject fourthObject;
    private GameObject firstObjectInstance;
    private GameObject secondObjectInstance;
    private GameObject thirdObjectInstance;
    private GameObject fourthObjectInstance;
    private bool isFirstObjectDestroyed = false;
    private bool isSecondObjectDestroyed = false;
    private bool allObjectDestroyed = false;
    public Canvas waveTitle;
    public Canvas hardTitle;
    private int counter = 0;
    private int counter1 = 0;

    void Start()
    {
        firstObjectInstance = Instantiate(firstObjectPrefab);

        Instantiate(waveTitle);

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
                Instantiate(hardTitle);
                thirdObjectInstance = Instantiate(thirdObject);
                fourthObjectInstance = Instantiate(fourthObject);
                allObjectDestroyed = true;
                
            }
        }

    if(firstObjectInstance == null && secondObjectInstance == null 
    && thirdObjectInstance == null && fourthObjectInstance == null)
    {
        Destroy(gameObject);
    }   
    }
}
