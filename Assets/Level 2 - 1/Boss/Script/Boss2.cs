using UnityEngine;

public class Boss2 : Enemy
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRate = 2f;
    public Transform[] firePoints;

    private float fireTimer;

    private void Start()
    {
        transform.position = new Vector3(88.15f, 98.11f, 0f);
        fireTimer = fireRate;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            FireProjectiles();
            fireTimer = fireRate;
        }
    }

    private void FireProjectiles()
    {
        float angleStep = 45f; 

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * angleStep; 

                // Convert the angle to a direction vector
                Vector3 direction = Quaternion.Euler(0f, 0f, angle) * firePoint.up;

                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                projectileMove.MoveDirection = direction.normalized; 
                projectileMove.MoveSpeed = projectileSpeed;

                if (Mathf.Approximately(angle, 90f) || Mathf.Approximately(angle, 270f))
                {
                    Destroy(projectile, 4f); 
                }
                else
                {
                    Destroy(projectile, 2f); 
                }
            }
        }
    }


}
