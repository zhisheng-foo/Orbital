using System.Collections;
using TMPro;
using UnityEngine;

public class TextTransitionBoss : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float transitionDuration = 1.0f;
    public float shakeIntensity = 0.1f;
    private Vector3 initialPosition;
    private void Start()
    {
        initialPosition = text.transform.position;
        StartCoroutine(TransitionText());
    }

    private IEnumerator TransitionText()
    {
        float elapsedTime = 0.0f;
        float startValue = text.alpha;
        float endValue = 1.0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);

            text.alpha = currentValue;

            // Add shaking effect
            float shakeX = Random.Range(-shakeIntensity, shakeIntensity);
            float shakeY = Random.Range(-shakeIntensity, shakeIntensity);
            text.transform.position = initialPosition + new Vector3(shakeX, shakeY, 0.0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.alpha = endValue;
        text.transform.position = initialPosition;
        Destroy(gameObject);
    }
}
