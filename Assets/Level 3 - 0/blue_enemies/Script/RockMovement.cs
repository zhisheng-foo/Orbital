using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//this class handles the projectiles of the rock throwing enemy
public class RockMovement : MonoBehaviour
{
    private Vector3 direction;
    private float throwForce;
    private float lifetime;

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
}
