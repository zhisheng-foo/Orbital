using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject firstObjectPrefab;

    private GameObject firstObjectInstance;
    private bool isFirstObjectDestroyed = false;
    public Canvas headTitle1;
    public Canvas headTitle2;
    public Canvas levelTitle1;
    public Canvas levelTitle2;
    public float updateDelay = 10.0f;
    public float victoryDelay = 10.0f;
    private int counter = 0;
    private string TRANSITION = "work";
    private Animator fadeAnimator;
    private GameObject fadeObject;
    void Start()
    {   
        fadeObject = GameObject.Find("Fade");
        StartCoroutine(StartDelay());

        if (fadeObject != null)
        {
            fadeAnimator = fadeObject.GetComponent<Animator>();
        }
    }

    IEnumerator StartDelay()
    {
        Instantiate(headTitle1); 
        Instantiate(headTitle2); 
        Instantiate(levelTitle1); 
        Instantiate(levelTitle2); 

        yield return new WaitForSeconds(updateDelay);
        StartUpdate(); 
    }

    void StartUpdate()
    {
        if (counter != 1)
        {
            isFirstObjectDestroyed = true;
            firstObjectInstance = Instantiate(firstObjectPrefab); 
            counter++;
        }
    }

    void Update()
    {
        Boss3_Manager boss_manager = GameObject.Find("Boss_Manager Variant Variant(Clone)").GetComponent<Boss3_Manager>();
        if (boss_manager.isDestroyed == true)
        {
            StartCoroutine(LoadVictorySceneAfterDelay()); 
        }
    }

    IEnumerator LoadVictorySceneAfterDelay()
    {
        yield return new WaitForSeconds(victoryDelay); 
    
        
        //check here
        GameObject[] tobedeletedObjects = GameObject.FindGameObjectsWithTag("ToBeD");
        GameObject[] fighterObjects = GameObject.FindGameObjectsWithTag("Fighter");
        GameObject[] objectsToDestroy = new GameObject[tobedeletedObjects.Length + fighterObjects.Length];
        tobedeletedObjects.CopyTo(objectsToDestroy, 0);
        fighterObjects.CopyTo(objectsToDestroy, tobedeletedObjects.Length);

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        
        fadeAnimator.SetBool(TRANSITION,true);

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Victory 1 - 0");
    }
}
