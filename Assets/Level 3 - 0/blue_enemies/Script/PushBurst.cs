using UnityEngine;
using UnityEngine.SceneManagement;

public class PushBurst : MonoBehaviour
{
    public float pushForce = 100f; 
    public float pushDistance = 10f; 
    public float pushDuration = 1.5f;
    public float cooldownDuration = 4f; 
    public float requiredDistance = 20f; 
    public AudioClip pushSound;
    private GameObject dustObject;
    private GameObject player;
    private AudioSource audioSource; 
    private bool isOnCooldown = false; 
    
    private void Start()
    {
        player = GameObject.Find("Player");
        dustObject = GameObject.Find("Dust");
        audioSource = GetComponent<AudioSource>();

        if (dustObject != null)
        {
            dustObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && SceneManager.GetActiveScene().name == "Level 3 - 0")
        {
            if (!isOnCooldown)
            {
                PerformPushBurst();
            }
            else
            {
                GameManager.instance.ShowText("Still in 5 seconds cooldown", 20, Color.black,
                    player.transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 25, 0.3f);
            }
        }
    }

    private void PerformPushBurst()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position,
         pushDistance);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Fighter") && collider.name != "Boss_3")
            {
                Vector2 direction = collider.transform.position - player.transform.position;
                float distance = Vector2.Distance(player.transform.position,
                 collider.transform.position);

                if (distance <= requiredDistance)
                {
                    GameManager.instance.ShowText("Push away your responsibilities", 20, Color.black,
                        player.transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 25, 0.3f);
                    StartCoroutine(ApplyPushGradually(collider, direction.normalized));
                    
                    // Show the "Dust" object
                    if (dustObject != null)
                    {
                        dustObject.SetActive(true);
                    }
                }
            }
        }

        audioSource.PlayOneShot(pushSound);
        StartCoroutine(StartCooldown());
    }

    private System.Collections.IEnumerator ApplyPushGradually(Collider2D collider, Vector2 direction)
    {
        float elapsedTime = 0f;
        Vector2 originalPosition = collider.transform.position;
        Vector2 targetPosition = originalPosition + (direction * pushDistance);

        while (elapsedTime < pushDuration)
        {
            float t = elapsedTime / pushDuration;
            collider.transform.position = Vector2.Lerp(originalPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dustObject.SetActive(false);
        collider.transform.position = targetPosition;
    }

    private System.Collections.IEnumerator StartCooldown()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(cooldownDuration);

        isOnCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, pushDistance);
        }
    }
}
