using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{   
    public TransitManager instance;
    public Button playButton;
    public AudioClip clickSound;
    public AudioSource audioSource;
    public GameObject objectToHide;
    public float hideDuration = 2.0f;

    private Color originalColor;
    private void Start()
    {
        originalColor = playButton.image.color;
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(HideObjectForFixedTime());
        StartCoroutine(StartWithAnyKey());
    }

    public void LobbyTransit() 
    {
        PlayClickSound();
        instance.transitionAnim.SetBool("work", true);
        StartCoroutine(FlashButton());
        StartCoroutine(Load());      
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.clip = clickSound;
            audioSource.Play();
            StartCoroutine(HideObjectForFixedTime());        
        }
    }

    private IEnumerator FlashButton()
    {
        playButton.image.color = new Color(0f, 0f, 0f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        playButton.image.color = originalColor;
    }

    private IEnumerator HideObjectForFixedTime()
    {
        objectToHide.SetActive(false);

        yield return new WaitForSeconds(hideDuration);

        objectToHide.SetActive(true);
    }

    private IEnumerator StartWithAnyKey()
    {
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            if (Input.anyKeyDown)
            {
                LobbyTransit();
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1.68f);
        SceneManager.LoadScene("Main");
    }
}

