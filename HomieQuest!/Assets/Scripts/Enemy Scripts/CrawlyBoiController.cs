using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlyBoiController : MonoBehaviour, EnemyLifeform {
    // Outgoing
    private int damage = 1;
    [SerializeField] private float knockback = 100f;

    // Internal
    private float maxHealth = 3, health;
    [SerializeField] private float knockbackMod = 1;
    [SerializeField] private float speed = 10f;

    private Vector2 dir = Vector2.left;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;

    void Awake() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start() {
        health = maxHealth;
    }

    void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void FixedUpdate() {
        rigidBody.AddForce(dir * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) // If collision is player
        {
            Vector2 KBDir = (collision.transform.position - transform.position).normalized;
            player.HitPlayer(damage, KBDir, knockback);
        }
    }

    public void HitEnemy(float damage, Vector2 knockbackDirection, float knockbackForce) {
        rigidBody.AddForce(knockbackDirection * knockbackForce * knockbackMod);
        TakeDamage(damage);
    }

    public void HitWall(float col) {
        if (col > 0) // Hit on right
        { // Turn left
            sprite.flipX = false;
            dir = Vector2.left;
        } else // Hit on left
        { // Turn right
            sprite.flipX = true;
            dir = Vector2.right;
        }
    }

    private void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) // Die
        {
            Destroy(gameObject);
        }
    }

    private void OnGameStateChanged(GameState newGameState) {
        GetComponent<Animator>().enabled = newGameState == GameState.Gaming;
        rigidBody.simulated = newGameState == GameState.Gaming;
        enabled = newGameState == GameState.Gaming;
    }
}
