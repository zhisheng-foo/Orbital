using UnityEngine;
using System.Collections;

public class ProjectileMovement : Collidable
{
    private Vector3 direction;
    private float throwForce;
    private float lifetime;
    private float slowDownDuration = 2f;

    public void Initialize(Vector3 direction, float throwForce, float lifetime)
    {
        this.direction = direction;
        this.throwForce = throwForce;
        this.lifetime = lifetime;
        StartMovement();
    }

    private void StartMovement()
    {
        StartCoroutine(MoveRoutine());
        StartCoroutine(DestroyAfterLifetime());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Move the projectile towards the specified direction with the given throw force
            transform.position += direction * throwForce * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    protected override void OnCollide(Collider2D other)
    {
        if (other.CompareTag("Fighter"))
        {
            if (other.name == "Player")
            {
                return;
            }

            Damage dmg = new Damage()
            {
                damageAmount = 1, // Set the desired damage amount
                origin = transform.position,
                pushForce = 0f // Set the desired push force (if applicable)
            };

            other.SendMessage("ReceiveDamage", dmg);

            // Apply slow down effect if enemy has the movement component
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && other.name != "Boss_3")
            {
                StartCoroutine(SlowDownEnemySpeed(enemy, other));
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }

        private IEnumerator SlowDownEnemySpeed(Enemy enemy, Collider2D other)
    {   
        // Store the original movement speed
        float originalYspeed = enemy.ySpeed;
        float originalXspeed = enemy.xSpeed;

        // Check if the speed reduction should occur (40% chance)
        if (Random.value <= 0.08f)
        {
            // Reduce the enemy's movement speed
            enemy.ySpeed /= 2f;
            enemy.xSpeed /= 2f;

            GameManager.instance.ShowText(
                    "Activated: Crippling Depression",
                    25,
                    Color.black,
                    transform.position + new Vector3(2.5f, 0f, 0f),
                    Vector3.up * 20,
                    0.3f);
                  
        }
       

        yield return new WaitForSeconds(slowDownDuration);

    }

}
