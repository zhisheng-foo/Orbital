using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            // Move the rock towards the specified direction with the given throw force
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
