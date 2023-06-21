using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 MoveDirection { get; set; } // Added MoveDirection property
    public float MoveSpeed { get; set; } = 5f; // Added default MoveSpeed value
    public float destroyDelay = 1f; // Delay before destroying the projectile

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
            Destroy(gameObject); // Destroy the projectile
        }
        else
        {
            transform.Translate(MoveDirection * MoveSpeed * Time.deltaTime); // Access MoveDirection and MoveSpeed properties
        }
    }



}
