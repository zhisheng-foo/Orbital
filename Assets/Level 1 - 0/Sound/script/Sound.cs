using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource bossMusic;
    public AudioSource victoryMusic;
    public string bossObjectName;
    private GameObject bossObject;
    private bool bossDestroyed = false;
    private bool isCrossfading = false;

    private void Start()
    {
        bossObject = GameObject.Find(bossObjectName);
        bossMusic.Play();
    }

    private void Update()
    {
        if (!bossDestroyed && bossObject == null)
        {
            bossDestroyed = true;
            StartCrossfade();
        }
    }

    private void StartCrossfade()
    {
        if (!isCrossfading)
        {
            StartCoroutine(Crossfade());
        }
    }

    private System.Collections.IEnumerator Crossfade()
    {
        isCrossfading = true;

        float fadeDuration = 0.5f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            bossMusic.volume = Mathf.Lerp(1.0f, 0.0f, t);
            victoryMusic.volume = Mathf.Lerp(0.0f, 0.4f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bossMusic.Stop();
        victoryMusic.Play();
        isCrossfading = false;
    }
}
