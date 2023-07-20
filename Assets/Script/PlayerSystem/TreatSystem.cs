using UnityEngine;
using UnityEngine.SceneManagement;

/*
Hides or Shows the treat counters for the UI depending on which scene the player class is in
*/


public class TreatSystem : MonoBehaviour
{
    private static TreatSystem instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;       
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start Game" || scene.name == "Victory 1 - 0")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}


