using UnityEngine;

public class HealthMaxPickup : MonoBehaviour {
    #region Unity Methods ================================================================================
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            player.HealthMaxPickup();
            Destroy(gameObject);
        }
    }
	#endregion
}
