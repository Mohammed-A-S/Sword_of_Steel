using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker_enemy : MonoBehaviour
{
    //health
    public int max_health = 100;
    int current_health;

    public GameObject death_Partical;

    //for target and the range of chasing
    public Transform player;
    float see_range = 7;

    //for moving
    bool isChasing;
    Rigidbody2D rb2d;
    Animator anim;
    float speed = 3f;

    //ground check
    public Transform groud_check;
    public LayerMask groundLayer;
    bool isGrounded;

    //attack
    public LayerMask enemy_layer;
    public Transform attack_point;
    public float attack_range = 0.5f;
    int damage = 100;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isChasing = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groud_check.position, 0.2f, groundLayer);

        if (player != null)
        {
            float distance_to_player = Vector2.Distance(transform.position, player.position);
            if (isChasing == true)
            {
                if (distance_to_player < see_range && isGrounded)
                {
                    if (transform.position.x < player.position.x)
                    {
                        //chasing right
                        rb2d.velocity = new Vector2(speed, 0f);
                        transform.localScale = new Vector2(0.7f, 0.7f);
                        anim.SetBool("move", true);

                        if (distance_to_player <= 1)
                        {
                            rb2d.velocity = Vector2.zero;
                            anim.SetBool("move", false);
                            anim.SetBool("attack", true);
                        }
                        else
                        {
                            anim.SetBool("attack", false);
                        }
                    }
                    else if (transform.position.x > player.position.x)
                    {
                        //chasing left
                        rb2d.velocity = new Vector2(-speed, 0f);
                        transform.localScale = new Vector2(-0.7f, 0.7f);
                        anim.SetBool("move", true);

                        if (distance_to_player <= 1)
                        {
                            rb2d.velocity = Vector2.zero;
                            anim.SetBool("move", false);
                            anim.SetBool("attack", true);
                        }
                        else
                        {
                            anim.SetBool("attack", false);
                        }
                    }
                }
                else
                {
                    rb2d.velocity = Vector2.zero;
                    anim.SetBool("move", false);
                }
            }
        }
    }

    public void skeleton_Take_damege(int damage)
    {
        GameObject death_partical = Instantiate(death_Partical, transform.position, Quaternion.identity);
        Destroy(death_partical, 2f);

        current_health -= damage;
        if (current_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layer);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<my_player>().Player_Take_damege(damage);
        }
    }
    private void OnDrawGizmosSelected() //for showing the attack range
    {
        Gizmos.DrawWireSphere(attack_point.position, attack_range);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }
}