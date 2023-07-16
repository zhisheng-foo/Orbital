using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeIn : MonoBehaviour
{
    public float fadeInDuration = 1f; 
    private Button button;
    private float currentTime = 0f;
    private bool isFading = false;
    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        button.image.canvasRenderer.SetAlpha(0f);
    }
    void Update()
    {
        if (isFading)
        {
            currentTime += Time.deltaTime;
            float alpha = currentTime / fadeInDuration;
            button.image.CrossFadeAlpha(alpha, 0f, true);

            if (currentTime >= fadeInDuration)
            {
                isFading = false;
                button.interactable = true;
            }
        }
    }
    public void StartFadeIn()
    {
        isFading = true;
        currentTime = 0f;
    }   
}
