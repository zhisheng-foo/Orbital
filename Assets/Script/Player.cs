using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private BoxCollider2D boxCollider;
   private Vector3 moveDelta;

   private Animator anim;

   private SpriteRenderer sr;

   private string WALK_ANIMATION = "Walk";

   private float x;
   private float y;

   protected float ySpeed = 5.0f;
    protected float xSpeed = 5.5f;

   

   private RaycastHit2D hit;

   private void Start() 
   {
     boxCollider = GetComponent<BoxCollider2D>();
     anim = GetComponent<Animator>();
   }

   private void FixedUpdate()
   {
        
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        
        //Reset MoveDelta
        moveDelta = new Vector3(x * xSpeed, y * ySpeed , 0);

        //Swap Direction
        if (moveDelta.x > 0) 
        {
            transform.localScale = Vector3.one;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);      
        }


        anim.SetBool(WALK_ANIMATION, true);

       if (x > 0 || y > 0) 
       {
            anim.SetBool(WALK_ANIMATION, true);
       }
       else if(x < 0 || y < 0) 
       {
            anim.SetBool(WALK_ANIMATION,true);
       }
       else 
       {
            anim.SetBool(WALK_ANIMATION,false);
       }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
        new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime),
        LayerMask.GetMask("Actor", "Blocking"));

        if (hit.collider == null) {

            transform.Translate(0, moveDelta.y * Time.deltaTime , 0);
            
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
        new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime),
        LayerMask.GetMask("Actor", "Blocking"));

         if (hit.collider == null) {

            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
            

        }      
   }

  
}
