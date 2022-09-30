using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_enemy : MonoBehaviour
{
    public int max_health = 100;
    int current_health;

    public GameObject death_Partical;

    //for target and the range of chasing
    public Transform player;
    float see_range = 13;

    //for moving
    bool isChasing;
    Rigidbody2D rb2d;

    //attack
    public Transform firepoint;
    public GameObject left_shoot;
    public GameObject right_shoot;
    Animator anim;
    float time_between;
    float start_time_between = 2f;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isChasing = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distance_to_player = Vector2.Distance(transform.position, player.position);
            if (isChasing == true)
            {
                if (distance_to_player < see_range)
                {
                    if (transform.position.x < player.position.x)
                    {
                        //chasing right
                        transform.localScale = new Vector2(-0.6f, 0.6f);

                        if (distance_to_player <= 11)
                        {
                            if (time_between <= 0)
                            {
                                anim.SetBool("attack", true);
                                Instantiate(left_shoot, firepoint.position, firepoint.rotation);
                                time_between = start_time_between;

                            }
                            else
                            {
                                time_between -= Time.deltaTime;
                                anim.SetBool("attack", false);
                            }
                        }
                    }
                    else if (transform.position.x > player.position.x)
                    {
                        //chasing left
                        transform.localScale = new Vector2(0.6f, 0.6f);

                        if (distance_to_player <= 11)
                        {
                            if (time_between <= 0)
                            {
                                anim.SetBool("attack", true);
                                Instantiate(right_shoot, firepoint.position, firepoint.rotation);
                                time_between = start_time_between;

                            }
                            else
                            {
                                time_between -= Time.deltaTime;
                                anim.SetBool("attack", false);
                            }
                        }
                    }
                }
            }
        }
    }

    public void witch_take_damege(int damage)
    {
        GameObject death_partical = Instantiate(death_Partical, transform.position, Quaternion.identity);
        Destroy(death_partical, 2f);

        current_health -= damage;
        if (current_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}