using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SameSceneFade : Collidable
{
    public Vector3 targetPosition;

    public List<GameObject> objectsToDestroy;
    public TextMeshProUGUI textMesh;
    public float collisionTextDuration = 2f;

    private Coroutine flashingCoroutine;
    private bool canCollide = true;
    private float collisionCooldown = 3f;

    public AudioSource audioSource;
    public float collisionAudioCooldown = 2f; // Cooldown duration in seconds

    private bool canPlayCollisionAudio = true;

    private GameObject player;
    private GameObject fadeObject;

    private string TRANSITION = "work";
    private string SAME_SCENE = "same";

    private Animator fadeAnimator;

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        fadeObject = GameObject.Find("Fade");

        if (fadeObject != null)
        {
            fadeAnimator = fadeObject.GetComponent<Animator>();
        }

        audioSource = GetComponent<AudioSource>();

        // Disable the textMesh object at the start
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (!canCollide || coll.name != "Player")
        {
            return;
        }

        if (!AllObjectsDestroyed())
        {
            textMesh.gameObject.SetActive(true);
            StartFlashing();
            StartCoroutine(HideTextMeshAfterDelay(collisionTextDuration));
            StartCoroutine(ActivateCooldown());
            return;
        }

        if (player != null && AllObjectsDestroyed())
        {
            if (fadeAnimator != null)
            {
                fadeAnimator.SetBool(TRANSITION, true);

                if (canPlayCollisionAudio)
                {
                    PlayCollisionAudio();
                    canPlayCollisionAudio = false;
                    StartCoroutine(ResetCollisionAudioCooldown());
                }

                StartCoroutine(WaitForFadeComplete());
            }
            else
            {
                TeleportPlayer();
            }
        }
    }

    private IEnumerator WaitForFadeComplete()
    {
        yield return new WaitForSeconds(1f); // Wait for the first animation to complete

        // Disable the first animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetBool(TRANSITION, false);
            TeleportPlayer();
        }

        // Play the black fade to normal animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetBool(SAME_SCENE, true);
        }

        yield return new WaitForSeconds(1f); // Wait for the black fade animation to complete
    }

    private void TeleportPlayer()
    {
        if (AllObjectsDestroyed())
        {
            player.transform.position = targetPosition;
        }
        else
        {
            Debug.Log("Cannot teleport player. All objects are not destroyed yet.");
        }
    }

    private void PlayCollisionAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator ResetCollisionAudioCooldown()
    {
        yield return new WaitForSeconds(collisionAudioCooldown);
        canPlayCollisionAudio = true;
    }

    private bool AllObjectsDestroyed()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                return false;
            }
        }

        return true;
    }

    private void StartFlashing()
    {
        if (flashingCoroutine == null)
        {
            flashingCoroutine = StartCoroutine(FlashText());
        }
    }

    private void StopFlashing()
    {
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }
    }

    private IEnumerator FlashText()
    {
        while (true)
        {
            textMesh.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            textMesh.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator HideTextMeshAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopFlashing();
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    private IEnumerator ActivateCooldown()
    {
        canCollide = false;
        yield return new WaitForSeconds(collisionCooldown);
        canCollide = true;
    }
}
