using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    int mainMenu = 0;
    int tutorial = 1;


    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.SetState(GameState.Gaming);
    }
    public void ToMainMenu() {
        SceneManager.LoadScene(mainMenu);
        GameStateManager.Instance.SetState(GameState.Gaming);
    }
    public void ToTutorial() {
        SceneManager.LoadScene(tutorial);
        GameStateManager.Instance.SetState(GameState.Gaming);
    }

    public void ToNextLevel() {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevel < SceneManager.sceneCountInBuildSettings ? nextLevel : mainMenu);
        GameStateManager.Instance.SetState(GameState.Gaming);
    }
}
