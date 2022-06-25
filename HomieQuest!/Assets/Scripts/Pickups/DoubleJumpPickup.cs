using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour {
    [SerializeField] bool test;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            player.UnlockDoubleJump();
            if (!test) AudioManager.Instance.PlaySFX("PowerUp");
            Destroy(gameObject);
        }
    }
}
