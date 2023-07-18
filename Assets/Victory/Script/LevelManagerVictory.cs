using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerVictory : MonoBehaviour
{
    public AudioClip BGM;
    public AudioClip ReZeroMonsterSpawn;
    public AudioClip convoaudio;
    private AudioSource audiosource;

    private Player player;

    void Awake()
    {
        /*
        //check here
        GameObject[] tobedeletedObjects = GameObject.FindGameObjectsWithTag("ToBeD");
        GameObject[] fighterObjects = GameObject.FindGameObjectsWithTag("Fighter");
        Debug.Log(tobedeletedObjects + "TBD " + fighterObjects + " figherobjects");
        GameObject[] objectsToDestroy = new GameObject[tobedeletedObjects.Length + fighterObjects.Length];
        tobedeletedObjects.CopyTo(objectsToDestroy, 0);
        fighterObjects.CopyTo(objectsToDestroy, tobedeletedObjects.Length);

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        */
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioClips());
        GameManager.instance.dollar = 0;
        player.hitpoint = player.maxHitpoint;
    }

    IEnumerator PlayAudioClips()
    {
        
        yield return new WaitForSeconds(9.3f); 
        audiosource.clip = convoaudio;
        audiosource.Play();

        yield return new WaitForSeconds(17.7f); 
        audiosource.clip = ReZeroMonsterSpawn;
        audiosource.Play();

        yield return new WaitForSeconds(15f); 
        SceneManager.LoadScene("Start Game");

        SceneManager.UnloadScene("Main");
        SceneManager.UnloadScene("Level 1 - 0");
    }
    
    void Update()
    {

    }
}
