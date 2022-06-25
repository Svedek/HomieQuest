using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour {
    //[SerializeField] GameObject door;
    [SerializeField] bool test;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            player.UnlockDash();
            if (!test) AudioManager.Instance.PlaySFX("PowerUp");
            /*DestroyDoor();*/
            Destroy(gameObject);
        }
    }
    /*
    private void DestroyDoor() {
        if (door == null) return;
        Destroy(door);
        AudioManager.Instance.PlaySFX("DoorOpen");
    }*/
}
