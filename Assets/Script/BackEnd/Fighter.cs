using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Basic class for all fighters in the game. Initalises the hitpoints of all fighters and immune time.
*/

public class Fighter : MonoBehaviour
{
    public int hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoverySpeed = 2.0f;
    protected float immuneTime = 1.0f;
    protected float lastImmune;
    public float knockTime;
    public Vector3 pushDirection;
    private Collider2D other;
    protected virtual void Start() {}
    protected virtual void ReceiveDamage(Damage dmg) {}
    protected virtual void Death() {}
}