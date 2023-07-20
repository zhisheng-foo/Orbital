using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Containts the damage information that is passed to all fighter classes.

public struct Damage 
{
    public Vector3 origin;
    public int damageAmount;
    public float pushForce;
}
