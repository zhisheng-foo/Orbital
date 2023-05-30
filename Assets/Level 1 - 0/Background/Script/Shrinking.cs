using UnityEngine;

public class Shrinking : MonoBehaviour
{
    public float shrinkSpeed = 0.5f; // Adjust this value to control the speed of shrinking

    private Vector3 initialScale;

    private void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Calculate the new scale based on the shrink speed and time
        float newScale = Mathf.Lerp(transform.localScale.x, 0f, Time.deltaTime * shrinkSpeed);

        // Apply the new scale to all axes
        transform.localScale = new Vector3(newScale, newScale, newScale);

        if (transform.localScale.x <= 0.4f)
        {
            
            Destroy(gameObject);
            GetComponent<Renderer>().enabled = false;
        }
    }
}
