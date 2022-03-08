using UnityEngine;

public class DeathBarrier : MonoBehaviour {
    #region Unity Methods ================================================================================
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) {
            player.HitPlayer(999, Vector2.up, 999);
        }
    }
    #endregion
}
