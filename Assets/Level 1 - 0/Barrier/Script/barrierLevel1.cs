using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class barrierLevel1 : Collidable
{
    public GameObject[] objects;
    public string deathAnimation = "DEATH";
    public TextMeshProUGUI textMesh;
    private string playerName = "Player";
    private bool isDestroyed = false;
    private GameObject playerObject;
    private Coroutine flashingCoroutine;
    private bool canCollide = true;
    private float collisionCooldown = 3f;

    protected override void Start()
    {
        base.Start();

        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }

        playerObject = GameObject.Find(playerName);
    }

    protected override void Update()
    {
        base.Update();

        if (isDestroyed)
        {
            return;
        }
        
        bool allObjectsDestroyed = true;
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                allObjectsDestroyed = false;
                break;
            }
        }

        if (allObjectsDestroyed)
        {
            isDestroyed = true;
            StartCoroutine(PlayDeathAnimationAndDestroy());

            canCollide = false;
        }
    }

    private IEnumerator PlayDeathAnimationAndDestroy()
    {

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool(deathAnimation, true);
        }
    
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }

    protected override void OnCollide(Collider2D coll)
    {
        
        if (!canCollide)
        {
            return;
        }
      
        if (coll.gameObject == playerObject && textMesh != null)
        {
            textMesh.gameObject.SetActive(true);
            StartFlashing();
            StartCoroutine(HideTextMeshAfterDelay(1f)); //was 2.4
            StartCoroutine(ActivateCooldown());
        }
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
