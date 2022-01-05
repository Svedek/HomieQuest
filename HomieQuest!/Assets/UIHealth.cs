using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    private string message = "Health: ";
    private UnityEngine.UI.Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<UnityEngine.UI.Text>();
    }

    public void SetHealthText(int health)
    {
        healthText.text = message + health;
    }
}
