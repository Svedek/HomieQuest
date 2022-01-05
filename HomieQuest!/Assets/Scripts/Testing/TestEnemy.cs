using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, EnemyLifeform
{
    private float maxHealth = 3, health;
    private float knockbackResistance = 1;

    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) // If collision is player
        {

        }
    }

    public void HitEnemy(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        rigidBody.AddForce(knockbackDirection * knockbackForce / knockbackResistance);
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) // Die
        {
            Destroy(gameObject);
        }
    }
}
