using System.Collections;
using TMPro;
using UnityEngine;

public class TextTransitionBossDefeated : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float typingSpeed = 0.1f;
    public float displayDuration = 2.0f;
    public float checkInterval = 0.1f; // Interval between boss object checks

    public string fullText;
    private Coroutine typingCoroutine;
    private bool bossDefeated = false; // Track the status of the boss

    private void Start()
    {
        text.enabled = false;
        fullText = text.text;
        text.text = "";

        StartCoroutine(CheckBossStatus());
    }

    private IEnumerator CheckBossStatus()
    {
        while (!bossDefeated)
        {
            GameObject bossObject = GameObject.Find("Boss");
            if (bossObject == null)
            {
                bossDefeated = true;
                DisplayText();
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void DisplayText()
    {
        text.enabled = true;
        typingCoroutine = StartCoroutine(TypeText());
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        Destroy(gameObject);
    }
}
