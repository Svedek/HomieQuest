using UnityEngine;

public class HealthPickup : MonoBehaviour {
    private int health = 3;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            player.HealthPickup(health);
            Destroy(gameObject);
        }
    }
}
