using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCollider : MonoBehaviour
{
    PlayerController player;


    void Awake() {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 9) player.HitGround();
    }
}
