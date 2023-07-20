using UnityEngine;

public class ProjectileMove : MonoBehaviour
{   

    //Instantiate the prjectiles of Boss 2 in the given direction
    public Vector3 MoveDirection { get; set; }
    public float MoveSpeed { get; set; } = 10f;
    public float destroyDelay = 6f;
    private float destroyTimer;

    private void Start()
    {
        destroyTimer = destroyDelay;
    }

    private void Update()
    {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(MoveDirection * MoveSpeed * Time.deltaTime);
        }
    }

    public void SetDestroyDelay(float delay)
    {
        destroyDelay = delay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Collision with player occurred.");
        }
    }
}

