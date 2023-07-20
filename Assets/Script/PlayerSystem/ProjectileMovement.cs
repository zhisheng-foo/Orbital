using UnityEngine;
using System.Collections;

/*
Controls the projectile shot by the player such as the duration and speed of the projectile

Inherits from collidable
*/

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
                damageAmount = 1, 
                origin = transform.position,
                pushForce = 0f 
            };

            other.SendMessage("ReceiveDamage", dmg);

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && other.name != "Boss_3")
            {
                StartCoroutine(SlowDownEnemySpeed(enemy, other));
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator SlowDownEnemySpeed(Enemy enemy, Collider2D other)
    {   
        
        float originalYspeed = enemy.ySpeed;
        float originalXspeed = enemy.xSpeed;
   
        if (Random.value <= 0.08f)
        {
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
