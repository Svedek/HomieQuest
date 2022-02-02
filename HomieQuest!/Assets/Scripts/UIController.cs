using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private UIHeartController heartController;

    private void Awake()
    {
        heartController = GetComponent<UIHeartController>();
    }

    public void SetHealthUI(int health)
    {
        heartController.UISetHealth(health);
    }
}
