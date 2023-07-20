using UnityEngine;
using System.Collections;
using TMPro;

/* 
Controls the canvas elements to display level titles as well as other information that 
the player may need to see/know as they load into the scene and hiding them after a set duration
*/

public class ObjectHider : MonoBehaviour
{
    public float hideDuration = 2f; // Duration in seconds for which the object will be hidden
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Hide the object when the scene is loaded
        StartCoroutine(HideAndAppear());
    }

    IEnumerator HideAndAppear()
    {
        // Hide the object
        textMeshPro.enabled = false;

        // Wait for the hide duration
        yield return new WaitForSeconds(hideDuration);

        // Make the object appear
        textMeshPro.enabled = true;
    }
}

