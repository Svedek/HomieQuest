using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private UIHealthController healthController;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuBG;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject loseMenuBG;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject victoryMenuBG;


    private void Awake() {
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

        // Defeat
        loseMenu.SetActive(newGameState == GameState.Lose);
        loseMenuBG.SetActive(newGameState == GameState.Lose);

        // Victory
        victoryMenu.SetActive(newGameState == GameState.Victory);
        victoryMenuBG.SetActive(newGameState == GameState.Victory);
    }
}
