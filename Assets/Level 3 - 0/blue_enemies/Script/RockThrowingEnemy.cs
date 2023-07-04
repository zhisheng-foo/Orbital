using System.Collections;
using UnityEngine;

public class RockThrowingEnemy : Enemy
{
    public GameObject rockPrefab;
    public GameObject droneObject; // Reference to the drone object
    public float throwForce = 10f;
    public float throwDelay = 1.2f;
    public float rockLifetime = 5f;

    private bool canThrow = true;
    private bool isAlive = true;

    private string THROW_ANIMATION = "Throw";
    private string DEATH_ANIMATION = "Death";
    private string DRONE_DEATH = "Death1"; // Drone death animation trigger name

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ThrowRockRoutine());
    }

    private IEnumerator ThrowRockRoutine()
    {
        while (true)
        {
            if (isAlive && canThrow)
            {
                // Instantiate a rock prefab
                GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);

                // Calculate the direction towards the player
                Vector3 direction = (playerTransform.position - transform.position).normalized;

                // Get the RockMovement component from the rock prefab
                RockMovement rockMovement = rock.GetComponent<RockMovement>();

                // Set the direction, throw force, and lifetime for the rock movement
                rockMovement.Initialize(direction, throwForce, rockLifetime);

                canThrow = false;

                // Trigger the throw animation
                anim.SetBool(THROW_ANIMATION, true);

                // Wait for the throw delay before allowing another throw
                yield return new WaitForSeconds(throwDelay);

                // Reset the throw animation
                anim.SetBool(THROW_ANIMATION, false);

                canThrow = true;
            }

            yield return null;
        }
    }

    protected override void Death()
    {
        base.Death();

        // Mark the enemy as not alive
        isAlive = false;

        // Trigger the death animation
        anim.SetBool(DEATH_ANIMATION, true);

        // Trigger the drone death animation
        Animator droneAnimator = droneObject.GetComponent<Animator>();
        if (droneAnimator != null)
        {
            droneAnimator.SetBool(DRONE_DEATH, true);
        }

        // Start the coroutine to handle death animation and object destruction
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1.9f);

        // Give treats to the player
        GameManager.instance.dollar += 4;
        int temp = 4;
        GameManager.instance.ShowText("+ " + temp + " TREATS", 15, new Color(1f, 0.0f, 0f), transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 40, 1.0f);

        // Destroy the object
        Destroy(gameObject);
    }
}
