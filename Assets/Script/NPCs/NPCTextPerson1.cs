using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson1 : Collidable
{
    public string message;

    private float cooldown = 0.4f;
    private float cooldown2 = 1.5f;
    private float lastShout;

    [SerializeField]
    private AudioSource voiceSoundEffect;

    private Animator animator;
    private bool isPlayerColliding = false;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
        animator = GetComponent<Animator>();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }

        if (Time.time - lastShout > cooldown2)
        {
            voiceSoundEffect.Play();
        }

        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 25, new Color(0f, 0f, 0f), transform.position + new Vector3(0, 0.85f, 0), Vector3.zero, cooldown * 1.05f);
        }

        if (!isPlayerColliding)
        {
            animator.SetBool("Collided", true);
            isPlayerColliding = true;
            StartCoroutine(ResetCollisionState());
        }
        else
        {
            // Reset the animation state so it loops
            animator.Play("Collided", -1, 0f);
        }
    }



    private IEnumerator ResetCollisionState()
    {
        while (true)
        {
            yield return null; // Wait for the next frame

            // Check if the collision with the player is still ongoing
            if (!IsCollidingWithPlayer())
            {
                animator.SetBool("Collided", false);
                isPlayerColliding = false;
                break; // Exit the coroutine
            }
        }
    }

    private bool IsCollidingWithPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Player")
            {
                return true;
            }
        }
        return false;
    }
}
