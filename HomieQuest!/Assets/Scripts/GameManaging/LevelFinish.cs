using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {
    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        player.LevelFinish();
        GameStateManager.Instance.SetState(GameState.Victory);
    }
    #endregion
}
