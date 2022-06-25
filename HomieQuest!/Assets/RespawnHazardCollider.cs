using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHazardCollider : MonoBehaviour {
    #region Fields ================================================================================
    private int damage = 1;
    #endregion

    #region Unity Methods ================================================================================
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            player.HitRespawnHazard(damage);
        }
    }
    #endregion
    }
