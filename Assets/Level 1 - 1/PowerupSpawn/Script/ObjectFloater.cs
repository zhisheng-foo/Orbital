using UnityEngine;


//help to float an object at a given position 
public class ObjectFloater : MonoBehaviour
{
    public float floatSpeed = 1f; 
    public float floatHeight = 0.10f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
