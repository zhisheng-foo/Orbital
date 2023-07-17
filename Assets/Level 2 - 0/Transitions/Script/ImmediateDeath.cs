using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImmediateDeath : Collidable
{
    private Player player;
    public AudioSource audioSource;
    private bool hasPlayedAudio;
    public TextMeshProUGUI textMesh; 
    public float displayDuration = 2f; 
    public string text;
    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        hasPlayedAudio = false;

        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player" && player != null)
        {
            player.transform.position = new Vector3(2f, 6f, 0); //to solve double death.
            if (audioSource != null && !hasPlayedAudio)
            {
                audioSource.Play();
                hasPlayedAudio = true;

                DisplayText();
            }
            player.hitpoint = 0;
        }
    }

    private void DisplayText()
    {
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(true);
            textMesh.text = text; 
            StartCoroutine(DestroyTextAfterDelay(displayDuration));
        }
    }

    private IEnumerator DestroyTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
            Destroy(textMesh.gameObject);
        }
    }
}
