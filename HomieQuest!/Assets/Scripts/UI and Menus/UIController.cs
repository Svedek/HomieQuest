using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private UIHealthController healthController;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuBG;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject loseMenuBG;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject victoryMenuBG;

    public static UIController Instance { get; private set; }

    private void Awake() {
        Instance = this;
        healthController = GetComponent<UIHealthController>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void SetHelthUI(int health) {
        healthController.UISetHealth(health);
    }
    public void SetHeartsUI(int hearts) {
        healthController.UISetHeartsActive(hearts);
    }

    public void SetLivesUI(int lives) {
        healthController.UISetLives(lives);
    }

    private void OnGameStateChanged(GameState newGameState) {
        // Paused
        pauseMenu.SetActive(newGameState == GameState.Paused);
        pauseMenuBG.SetActive(newGameState == GameState.Paused);
        if (newGameState != GameState.Paused) settingsMenu.SetActive(false);

        // Defeat
        loseMenu.SetActive(newGameState == GameState.Lose);
        loseMenuBG.SetActive(newGameState == GameState.Lose);

        // Victory
        victoryMenu.SetActive(newGameState == GameState.Victory);
        victoryMenuBG.SetActive(newGameState == GameState.Victory);
    }


    public void Resume() {
        GameStateManager.Instance.SetState(GameState.Gaming);
    }
    public void NextLevel() {
        LevelManager.Instance.ToNextLevel();
    }
    public void Exit() {
        LevelManager.Instance.ToMainMenu();
    }
    public void Retry() {
        LevelManager.Instance.ReloadLevel();
    }
}
