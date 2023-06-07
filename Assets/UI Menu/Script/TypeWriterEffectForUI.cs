using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterEffectForUI : MonoBehaviour
{
    public float delay = 0.1f;
    private TextMeshProUGUI textMeshPro;
    public string fullText = "";
    private string currentText = "";
    public AudioSource audioSource;
    private bool isTyping = false;
    private int pressCount = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pressCount++;

            if (pressCount % 2 == 1)
            {
                isTyping = true;
                StartCoroutine(PlayTypewriterEffect());
            }
            else
            {
                isTyping = false;
                ResetTypewriterEffect();
            }
        }
    }

    private void ResetTypewriterEffect()
    {
        currentText = "";
        textMeshPro.text = currentText;
    }

    private IEnumerator PlayTypewriterEffect()
    {
        float timer = 0f;

        for (int i = currentText.Length; i <= fullText.Length; i++)
        {
            if (isTyping)
            {
                currentText = fullText.Substring(0, i);
                textMeshPro.text = currentText;
            }

            timer += Time.unscaledDeltaTime;

            while (timer < delay)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
            }

            timer -= delay;
        }
    }
}
