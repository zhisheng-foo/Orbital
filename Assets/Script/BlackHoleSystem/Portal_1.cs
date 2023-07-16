using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_1 : Collidable
{
    public Vector3 desiredPlayerPosition = new Vector3(-0.05f, 7.17f, 0.0f); 
    private string level1_0 = "Level 1 - 0";
    private string TRANSITION = "Transit";
    private Animator anim;
    public AudioSource audioSource;
    private bool audioPlayed = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player" && !audioPlayed)
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

        SceneManager.LoadScene(level1_0);
          
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerObject.transform.position = desiredPlayerPosition;
        }
    }
}
