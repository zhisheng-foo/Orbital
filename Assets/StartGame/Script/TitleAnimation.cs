using System.Collections;
using TMPro;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float delay = 0.1f;
    private TextMeshProUGUI textMeshPro;
    public string fullText = "";
    private string currentText = "";
 
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        StartCoroutine(PlayTypewriterEffect());
    }

    private IEnumerator PlayTypewriterEffect()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textMeshPro.text = currentText;

            yield return new WaitForSeconds(delay);
        }
    }
}
