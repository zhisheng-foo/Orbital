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

    public float updateDelay = 10.0f; // Delay in seconds before Update method is called
    public float victoryDelay = 10.0f; // Delay in seconds before loading the victory scene

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
        Instantiate(headTitle1); // Instantiate the first head title canvas
        Instantiate(headTitle2); // Instantiate the second head title canvas
        Instantiate(levelTitle1); // Instantiate the first level title canvas
        Instantiate(levelTitle2); // Instantiate the second level title canvas

        yield return new WaitForSeconds(updateDelay);
        StartUpdate(); // Start the update after the delay
    }

    void StartUpdate()
    {
        if (counter != 1)
        {
            isFirstObjectDestroyed = true;
            firstObjectInstance = Instantiate(firstObjectPrefab); // Instantiate the first object
            counter++;
        }
    }

    void Update()
    {
        Boss3_Manager boss_manager = GameObject.Find("Boss_Manager Variant Variant(Clone)").GetComponent<Boss3_Manager>();
        if (boss_manager.isDestroyed == true)
        {
            StartCoroutine(LoadVictorySceneAfterDelay()); // Start the coroutine to load the victory scene
        }
    }

    IEnumerator LoadVictorySceneAfterDelay()
    {
        yield return new WaitForSeconds(victoryDelay); // Wait for the specified delay before loading the scene
        fadeAnimator.SetBool(TRANSITION,true);

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Victory 1 - 0"); // Load the victory scene
    }
}
