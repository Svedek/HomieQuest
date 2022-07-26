using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private int mainMenu = 0;
    private int tutorial = 1;

    public static LevelManager Instance { get; private set; }

    private void Awake() {
        Instance = this; // I know this is bad lol
    }

    private void Start() {
        PlayProperSongLol();
    }


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

    private void PlayProperSongLol() {
        int nextLevel = SceneManager.GetActiveScene().buildIndex;
        switch (nextLevel) {
            case 0:
                // Main menu, shouldn't happen
                AudioManager.Instance.PlayMusic("TitleSong");
                break;

            case int n when (n >= tutorial && n <= tutorial + 4):
                // Seg 1
                AudioManager.Instance.PlayMusic("Seg1Song");
                break;

            case int n when (n >= tutorial + 5 && n <= tutorial + 8):
                // Seg 2
                AudioManager.Instance.PlayMusic("Seg2SongNoIntro");
                break;

            case int n when (n >= tutorial + 9 && n <= tutorial + 12):
                // Seg 3
                break;

            case int n when (n >= tutorial + 13 && n <= tutorial + 16):
                // Seg 4
                break;

            default:
                print("Idk how this happen LevelManager.cs");
                break;
        }
    }
}
