using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_skeleton : MonoBehaviour
{
    public int max_health = 100;
    int current_health;

    bool isMoving;
    float speed = 2f;
    Rigidbody2D rb2d;
    bool ismoving_right;
    
    //raycast
    public Transform rayCast;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask playerMask;

    public Transform firepoint;
    Animator anim;
    float time_between;
    float start_time_between = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        ismoving_right = true;
        current_health = max_health;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(isMoving == true)
        {
            if (ismoving_right == true)
            {
                transform.localScale = new Vector2(0.7f, 0.7f);
                rb2d.velocity = new Vector2(speed, 0f);
            }
            else
            {
                transform.localScale = new Vector2(-0.7f, 0.7f);
                rb2d.velocity = new Vector2(-speed, 0f);
            }
        }

        hit = Physics2D.Raycast(transform.position, transform.right, distance, playerMask);
        Debug.DrawRay(rayCast.position, Vector2.right * distance, Color.red);

        if (hit.collider != null)
        {
            if (time_between <= 0)
            {
                anim.SetBool("attack", true);
                time_between = start_time_between;
                isMoving = false;
            }
            else
            {
                time_between -= Time.deltaTime;
                anim.SetBool("attack", false);
                isMoving = true;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "flip")
        {
            if(ismoving_right == true)
            {
                ismoving_right = false;
            }
            else
            {
                ismoving_right = true;
            }
        }
    }
    public void Take_damege(int damage)
    {
        current_health -= damage;
        if (current_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}






/*
 hit = Physics2D.Raycast(transform.position, transform.right, distance, playerMask);
        Debug.DrawRay(rayCast.position, Vector2.right * distance, Color.red);

        if (hit.collider != null)
        {
            if (time_between <= 0)
            {
                anim.SetBool("attack", true);
                time_between = start_time_between;

            }
            else
            {
                time_between -= Time.deltaTime;
                anim.SetBool("attack", false);
            }
        }
 */