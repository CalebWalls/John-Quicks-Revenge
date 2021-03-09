using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight =15f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    
  
    private bool faceLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        
    }
    private void Update()
    {
        //transition from jump to fall
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }
        //transition from fall to idle
        if(coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }

    }

    private void Move()
    {
        if (faceLeft)
        {
            //Test to see if we are beyond the leftCap
            if (transform.position.x > leftCap)
            {
                //make sure sprite is facing right location, and if not face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // Test to see if I am on ground if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }
            }

            else
            {
                faceLeft = false;
            }
            //if it is not we are going to face right

        }

        else
        {
            if (transform.position.x < rightCap)
            {
                //make sure sprite is facing right location, and if not face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // Test to see if I am on ground if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }
            }

            else
            {
                faceLeft = true;
            }

        }
    }









    
     
    
}
