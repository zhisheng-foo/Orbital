using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    public float bossBattleScale = 1.5f;
    public float scaleSpeed = 2f; // Speed of the scaling effect
    private float originalSize;
    private GameObject boss;
    private bool isInBossBattle = false;
    private Coroutine scalingCoroutine;

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
        originalSize = Camera.main.orthographicSize;
        boss = GameObject.Find("Boss"); // Replace "Boss" with the actual name of your boss GameObject
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;
        float deltaX = lookAt.position.x - transform.position.x;

        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        float deltaY = lookAt.position.y - transform.position.y;

        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);

        // Check if the player has crossed a certain y-value and if the boss is present
        if (!isInBossBattle && transform.position.y > 17 && boss != null)
        {
            SetBossBattle(true);
        }
        else if (isInBossBattle && boss == null)
        {
            SetBossBattle(false);
        }
    }

    public void SetBossBattle(bool isBossBattle)
    {
        if (isBossBattle == isInBossBattle)
        {
            return; // No need to re-trigger the scaling effect if already in the desired state
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
