using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitPortal : Collidable
{
    public string level = "Level 1 - 1"; // Updated scene name
    private string TRANSITION = "work";
    private Animator anim;
    public AudioSource audioSource;

    public TextMeshProUGUI textMesh;
    public float collisionTextDuration = 2f;

    private Coroutine flashingCoroutine;
    private bool audioPlayed = false;
    private bool portalEnabled = true;
    public float cooldownDuration = 3f;

    public GameObject[] objectsToDestroy;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnCollide(Collider2D coll)
    {   
        if (coll.name == "Weapon" && !audioPlayed && portalEnabled)
        {
            GameObject fadeObject = GameObject.Find("Fade");
            if (fadeObject != null)
            {
                if (AllObjectsDestroyed())
                {
                    audioSource.Play();
                    audioPlayed = true;
                    anim = fadeObject.GetComponent<Animator>();
                    StartCoroutine(TransitionToSceneWithDelay());

                    GameObject playerObject = coll.gameObject;
                    DontDestroyOnLoad(playerObject);
                }
                else
                {
                    textMesh.gameObject.SetActive(true);
                    StartFlashing();
                    StartCoroutine(HideTextMeshAfterDelay(collisionTextDuration));
                }

                DisablePortalForCooldown();
            }
        }
    }

    private IEnumerator TransitionToSceneWithDelay()
    {
        anim.SetBool(TRANSITION, true);
        yield return new WaitForSeconds(1.2f);

        // Load the new scene
        SceneManager.LoadScene(level); // Load "Level 1 - 1" scene
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

    private void DisablePortalForCooldown()
    {
        portalEnabled = false;
        StartCoroutine(EnablePortalAfterCooldown());
    }

    private IEnumerator EnablePortalAfterCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        portalEnabled = true;
    }
}
