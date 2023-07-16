using UnityEngine;

public class FloatingTextManager_0 : MonoBehaviour
{
    private static FloatingTextManager_0 instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

