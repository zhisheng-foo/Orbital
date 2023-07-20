using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This class handles the sequence in the level
//follows a destroy - then instantiate approach
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

    private Player player;

    void Start()
    {   
        player = GameObject.Find("Player").GetComponent<Player>();
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
        Boss3_Manager boss_manager 
        = GameObject.Find("Boss_Manager Variant Variant(Clone)").GetComponent<Boss3_Manager>();
        if (boss_manager.isDestroyed == true)
        {
            StartCoroutine(LoadVictorySceneAfterDelay()); 
        }
    }

    IEnumerator LoadVictorySceneAfterDelay()
    {
        yield return new WaitForSeconds(victoryDelay); 
    
        
        fadeAnimator.SetBool(TRANSITION,true);

        yield return new WaitForSeconds(3.0f);
        player.transform.position = new Vector3(100000000000000000f, 1000000000000000f, 0);


        SceneManager.LoadScene("Victory 1 - 0");

    }
}
