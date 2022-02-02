using UnityEngine;

public class ChakramController : MonoBehaviour
{
    [SerializeField] float damage, knockback, velocity;
    [SerializeField] int turnDistance, maxDistance;

    private Rigidbody2D rb;
    private PlayerController playerController;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (--maxDistance == turnDistance) rb.velocity = rb.velocity * -1;
        if (maxDistance <= 0) DestryChakram();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null) // Collision was player
        {
            if (player.ChakramCollide()) // If player was grounded
            {
                DestryChakram();
            }
            return;
        }

        EnemyLifeform enemy = collision.GetComponent<EnemyLifeform>();
        if (enemy != null) // Collision was enemy
        {
            enemy.HitEnemy(damage, rb.velocity.normalized, knockback);
            /* Commented out turn on enemy hit
            if (turnDistance < maxDistance) // If has not already turned
            {
                rb.velocity = rb.velocity * -1;
                turnDistance = maxDistance + 1; // Turn distance is set to greater than the remaining distance so no more turns will happen 
            }
            */
            return;
        }

        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9) // If collision was Terrain
        {
            if (turnDistance < maxDistance) // If has not already turned
            {
                rb.velocity = rb.velocity * -1;
                turnDistance = maxDistance + 1; // Turn distance is set to greater than the remaining distance so no more turns will happen 
            } else
            {
                DestryChakram();
            }
            return;
        }
    }

    // Takes normalized vector to set initial velocity
    public void InitializeChakram(Vector2 dir, PlayerController player)
    {
        rb.velocity = dir * velocity;
        playerController = player;
    }

    private void DestryChakram()
    {
        playerController.ChakramDestroyed();
        Destroy(gameObject);
    }
}
