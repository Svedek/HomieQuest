using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private UIHealth health;

    // Start is called before the first frame update
    void Start()
    {
       health = GetComponentInChildren<UIHealth>();

    }

    public void SetHealthUI(int newHealth)
    {
        health.SetHealthText(newHealth);
    }
}
