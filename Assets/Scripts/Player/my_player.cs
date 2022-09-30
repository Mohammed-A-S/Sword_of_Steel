using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class my_player : MonoBehaviour
{
    public GameObject deathUI;


    Rigidbody2D rb2d;
    Animator anim;

    //death
    public GameObject death_partical;

    //Health
    int health = 100;
    int current_health;

    //sounds
    public AudioSource sound_manager;
    public AudioClip jump_sound, attack_sound, enemy_kill_sound, dash_sound;

    //for Ground Check
    public Transform groud_check;
    public LayerMask groundLayer;
    bool isGrounded;

    //Moving and Jump
    bool isMoving;
    public ParticleSystem dust;
    float speed = 8;


    //for jumping after bassing the edge of only 0.2 sec
    float jump = 16;
    private float coyoteTime = 0.5f;
    private float coyoteTimeCounter;

    //Dash
    public GameObject dash;
    float dashSpeed = 40f;
    float dashlength = 0.1f;
    float dashCooldown = 0.8f;
    float activeSpeed;
    float dashCounter;
    float dashCoolCounter;

    //Attack
    public LayerMask enemy_layer;
    public Transform attack_point;
    public float attack_range = 0.5f;
    int attack_damage = 100;
    float attack_rate = 2f;
    float next_attack = 0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        deathUI.SetActive(false);
        current_health = health;

        activeSpeed = speed;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();
        Dash();
    }



    void Move()
    {
        if (isMoving == true)
        {
            isGrounded = Physics2D.OverlapCircle(groud_check.position, 0.2f, groundLayer);
            float horezontal = Input.GetAxis("Horizontal");

            if (horezontal > 0)
            {
                rb2d.velocity = new Vector2(horezontal * activeSpeed, rb2d.velocity.y); //for do the move with help of the power of speed
                transform.localScale = new Vector2(1f, 0.8f); //for rotate the player to the way of run
                anim.SetFloat("move", 1f); //for play animation with parameter float
                if (isGrounded)
                {
                    dust.Play();
                }
            }
            else if (horezontal < 0)
            {
                rb2d.velocity = new Vector2(horezontal * activeSpeed, rb2d.velocity.y);
                transform.localScale = new Vector2(-1f, 0.8f);
                anim.SetFloat("move", 1f);
                if (isGrounded)
                {
                    dust.Play();
                }
            }
            else if (horezontal == 0)
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                anim.SetFloat("move", 0f);
            }

            // falling animation
            if (!isGrounded)
            {
                anim.SetBool("fall", true);
            }
            else
            {
                anim.SetBool("fall", false);
            }
        }
    }



    void Jump() //the ground layer should be sign
    {
        //for jumping after bassing the edge of only 0.2 sec
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKey("w") && coyoteTimeCounter > 0)
        {
            rb2d.velocity = Vector2.up * jump;
            anim.SetBool("jump", true);
            dust.Play();
            coyoteTimeCounter = 0f;

            sound_manager.clip = jump_sound;
            sound_manager.Play();
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }



    void Attack()
    {
        if (Time.time >= next_attack)
        {
            if (Input.GetMouseButton(0))
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);

                anim.SetBool("attack", true);
                next_attack = Time.time + 1f / attack_rate;

                Collider2D[] targets = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layer);
                foreach (Collider2D target in targets)
                {
                    if(target.GetComponent<Shooter_enemy>())
                    {
                        target.GetComponent<Shooter_enemy>().witch_take_damege(attack_damage);
                        StartCoroutine(timescale());
                    }
                    else if(target.GetComponent<Attacker_enemy>())
                    {
                        target.GetComponent<Attacker_enemy>().skeleton_Take_damege(attack_damage);
                        StartCoroutine(timescale());
                    }
                }
                sound_manager.clip = attack_sound;
                sound_manager.Play();
            }
            else
            {
                anim.SetBool("attack", false);
            }
        }
        
    }
    private void OnDrawGizmosSelected() //for showing the attack range
    {
        Gizmos.DrawWireSphere(attack_point.position, attack_range);
    }
    IEnumerator timescale()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = 1;
    }




    void Dash()
    {
        float horezontal = Input.GetAxis("Horizontal");
        if (horezontal != 0)
        {
            if (Input.GetKey("space"))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeSpeed = dashSpeed;
                    dashCounter = dashlength;

                    GameObject dash_partical = Instantiate(dash, transform.position, Quaternion.identity);
                    Destroy(dash_partical, 1f);

                    sound_manager.clip = dash_sound;
                    sound_manager.Play();
                }
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeSpeed = speed;
                dashCoolCounter = dashCooldown;
            }
        }
        
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    public void Player_Take_damege(int damage)
    {
        GameObject death_Partical = Instantiate(death_partical, transform.position, Quaternion.identity);
        Destroy(death_Partical, 2f);

        current_health -= damage;
        if (current_health <= 2)
        {
            Destroy(gameObject);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            deathUI.SetActive(true);
        }
    }
}