using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {
    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>() == null) return;
        GameStateManager.Instance.SetState(GameState.Victory);
    }
    #endregion
}
