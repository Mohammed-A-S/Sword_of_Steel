using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class left_shoot : MonoBehaviour
{
    float speed = 10f;
    Rigidbody2D rb;
    

    public LayerMask enemy_layer;
    public Transform attack_point;
    public float attack_range = 0.5f;
    int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.right * speed;
        Destroy(gameObject, 4f);
    }

    void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layer);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<my_player>().Player_Take_damege(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Attack();
            Destroy(gameObject);
        }
    }
}
