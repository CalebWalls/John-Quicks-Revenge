using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource explosion;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        explosion = GetComponent<AudioSource>();
    }
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        explosion.Play();
        rb.velocity = Vector2.zero;
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
}
