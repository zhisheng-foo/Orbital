using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : Collidable
{
    public string level = "Level 1 - 1"; // Updated scene name
    private string TRANSITION = "work";
    private Animator anim;
    public AudioSource audioSource;

    private bool audioPlayed = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnCollide(Collider2D coll)
    {   
        if (coll.name == "Weapon"&& !audioPlayed)
        {   
            GameObject fadeObject = GameObject.Find("Fade");
            if (fadeObject != null)
            {
                audioSource.Play();
                audioPlayed = true;
                anim = fadeObject.GetComponent<Animator>();
                StartCoroutine(TransitionToSceneWithDelay());

                GameObject playerObject = coll.gameObject;
                DontDestroyOnLoad(playerObject);
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
}
