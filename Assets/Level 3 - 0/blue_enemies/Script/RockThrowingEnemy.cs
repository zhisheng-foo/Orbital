using System.Collections;
using UnityEngine;
//this class handles the logic for enemies that throw projectiles
public class RockThrowingEnemy : Enemy
{
    public GameObject rockPrefab;
    public GameObject droneObject; 
    public float throwForce = 10f;
    public float throwDelay = 1.2f;
    public float rockLifetime = 5f;

    private bool canThrow = true;
    private bool isAlive = true;

    private string THROW_ANIMATION = "Throw";
    private string DEATH_ANIMATION = "Death";
    private string DRONE_DEATH = "Death1"; 
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ThrowRockRoutine());
    }

    //this class reference to RockMovement
    //Instantiate a projectile that is directted at the player's position
    private IEnumerator ThrowRockRoutine()
    {
        while (true)
        {
            if (isAlive && canThrow)
            {
                
                GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity); 
                Vector3 direction = (playerTransform.position - transform.position).normalized;  
                RockMovement rockMovement = rock.GetComponent<RockMovement>(); 
                rockMovement.Initialize(direction, throwForce, rockLifetime);
                canThrow = false;
                anim.SetBool(THROW_ANIMATION, true);
  
                yield return new WaitForSeconds(throwDelay);
 
                anim.SetBool(THROW_ANIMATION, false);
                canThrow = true;
            }

            yield return null;
        }
    }

    protected override void Death()
    {
        base.Death();
        isAlive = false;      
        anim.SetBool(DEATH_ANIMATION, true);
 
        Animator droneAnimator = droneObject.GetComponent<Animator>();
        if (droneAnimator != null)
        {
            droneAnimator.SetBool(DRONE_DEATH, true);
        }

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1.9f);

        GameManager.instance.dollar += 4;
        int temp = 4;
        GameManager.instance.ShowText("+ " + temp + " TREATS",
         15, new Color(1f, 0.0f, 0f), transform.position + new Vector3(2f, 0f, 0f), Vector3.up * 40, 1.0f);

        Destroy(gameObject);
    }
}
