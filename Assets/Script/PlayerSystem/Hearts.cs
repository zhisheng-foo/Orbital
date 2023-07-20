using UnityEngine;
using UnityEngine.SceneManagement;

/*
UI element to provide a visual representation of the player's current and max hitpoint.
*/

public class Hearts : MonoBehaviour
{
    private static Hearts instance;
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


