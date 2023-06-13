using UnityEngine;

public class ObjectFloater : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed of the float movement
    public float floatHeight = 0.10f; // Height of the float movement

    private Vector3 initialPosition; // Initial position of the object

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new Y position based on a sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Update the object's position with the new Y value
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
