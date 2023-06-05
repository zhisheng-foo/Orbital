using UnityEngine;
using System.Collections;
using TMPro;

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

