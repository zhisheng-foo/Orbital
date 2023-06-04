using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 3f;

    void Start()
    {
        // Invoke the DestroyObject function after the specified destroyTime
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        // Destroy the game object this script is attached to
        Destroy(gameObject);
    }
}
