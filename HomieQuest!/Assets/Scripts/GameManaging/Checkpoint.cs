using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    #region fields
    [SerializeField] private Color activationColor;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        if (player.SetCheckpoint(gameObject.GetComponentInParent<Transform>().position)) {
            gameObject.GetComponentInParent<ParticleSystem>().Play();
            gameObject.GetComponentsInParent<SpriteRenderer>()[1].color = activationColor;
        }
    }
    #endregion
}
