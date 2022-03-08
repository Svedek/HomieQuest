using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Pause system from https://www.youtube.com/watch?v=KPaEnLpu57s&list=PL4Iem3ybPvwhlofFMU7XKo9xlDhLZG-TD&index=63
public class PauseController : MonoBehaviour {
    void Update() {
        if (GameStateManager.Instance.CurrentGameState > GameState.Paused) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gaming
                ? GameState.Paused
                : GameState.Gaming;

            GameStateManager.Instance.SetState(newGameState);
        }
    }

    public void ResumeGame() {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
            GameStateManager.Instance.SetState(GameState.Gaming);
    }
}

