using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerController : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    

   

    //FSM
    private enum State {idle, running, jumping,falling,hurt}
    private State state = State.idle;
    

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 15f;
    [SerializeField] private int diamond = 0;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource diamondsound;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private int health;
    [SerializeField] private TextMeshProUGUI healthAmount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        healthAmount.text = health.ToString();

    }



    private void Update()
    {

        if(state != State.hurt)
        {
            Movement();
        }
        Movement();

        VelocitySwitch();
        anim.SetInteger("state", (int)state);//sets animation based on Enumerator state

    }


    //Diamond system
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "collectable")
        {
            
            Destroy(collision.gameObject);
            diamondsound.Play();
            diamond += 1;
            diamondText.text = diamond.ToString();
        }
    }

    //frog
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemy" )
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HealthManager();//Deals with health and will reset level if health is equal to 0
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is to my right therefore I should be damaged and moved left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //enemy is to my left therefore I should be damaged and moved right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }

            }
        }
    }

    private void HealthManager()
    {
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //makes you face left
            transform.localScale = new Vector2(-1, 1);

        }
        //moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //makes you face right
            transform.localScale = new Vector2(1, 1);

        }



        //jumping
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))

        {
            Jump();
        }
    }





    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
        state = State.jumping;
    }


    private void VelocitySwitch()
    {
        if (state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if(Mathf.Abs(rb.velocity.x )> 2f)
        {
            //Movieng
            state = State.running;
        }

        else
        {
            state = State.idle;
        }
       
    }



    private void Footstep()
    {
        footstep.Play();
    }

}
