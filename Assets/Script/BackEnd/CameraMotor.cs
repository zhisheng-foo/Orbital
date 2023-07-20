using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Ensures that the camera is correctly sized and follows the player class.
*/

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    public float bossBattleScale = 1.5f;
    public float scaleSpeed = 2f; 
    private float originalSize;
    private GameObject boss;
    private bool isInBossBattle = false;
    private Coroutine scalingCoroutine;
    public float crossingValue = 17f;
    private Vector3 smoothVelocity = Vector3.zero;
    public float smoothTime = 0.1f; 
    public float bossBattleSmoothTime = 0.05f; 

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
        originalSize = Camera.main.orthographicSize;
        boss = GameObject.Find("Boss"); 

        if (GameObject.Find("Boss") == null)
        {
            boss = GameObject.Find("Boss_2");
        }       
    }

    private void LateUpdate()
    {
        if (lookAt != null)
        {
            Vector3 targetPosition = lookAt.position;
            targetPosition.z = transform.position.z; 
     
            float currentSmoothTime = isInBossBattle ? bossBattleSmoothTime : smoothTime;

            transform.position = Vector3.SmoothDamp(transform.position, 
            targetPosition, ref smoothVelocity, currentSmoothTime);

            if (SceneManager.GetActiveScene().name == "Level 3 - 0")
            {
                SetBossBattle(true);
            } 
                   
            if (!isInBossBattle && transform.position.y > crossingValue && boss != null)
            {
                SetBossBattle(true);
            }
            else if (isInBossBattle && boss == null)
            {
                SetBossBattle(false);
            }
        }
    }

    public void SetBossBattle(bool isBossBattle)
    {
        if (isBossBattle == isInBossBattle)
        {
            return; 
        }

        isInBossBattle = isBossBattle;

        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
        }

        scalingCoroutine = StartCoroutine(ScaleCamera(isBossBattle ? bossBattleScale : 1f));
    }

    private IEnumerator ScaleCamera(float targetScale)
    {
        float currentScale = Camera.main.orthographicSize;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * scaleSpeed;
            float newScale = Mathf.Lerp(currentScale, originalSize * targetScale, timer);
            Camera.main.orthographicSize = newScale;
            yield return null;
        }
        
        scalingCoroutine = null;
    }
}




