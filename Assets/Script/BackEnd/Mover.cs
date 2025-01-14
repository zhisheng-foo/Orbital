using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Handles movement of all objects that move. 
Example: Enemy, Player and Boss classes will inherit form mover. 
Inherits from Fighter class.
*/


public abstract class Mover : Fighter  
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    public float ySpeed = 5.0f;   //base movespeed
    public float xSpeed = 5.5f;
    protected virtual void Start() {

        boxCollider = GetComponent<BoxCollider2D>();

    }

    protected virtual void FixedUpdate() {
        
    }

//Update of gameobject's position to simulate moving
    protected virtual void UpdateMotor(Vector3 input) { 

        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        
        if (moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        } 

        else if (moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1); 
        }

        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection , Vector3.zero, pushRecoverySpeed);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
         new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime),
         LayerMask.GetMask("Actor", "Blocking"));

        if (hit.collider == null) {

            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
         new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime),
         LayerMask.GetMask("Actor", "Blocking"));

        if (hit.collider == null) {

            transform.Translate(moveDelta.x * Time.deltaTime,0, 0);

        }
    }
}
