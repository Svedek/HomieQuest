using UnityEngine;
using UnityEngine.UI;

public class UIHeartController : MonoBehaviour
{
    private int health = 6;
    private int heartsActive = 3;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    public void UISetHealth(int newHealth)
    {
        health = newHealth;
        UpdateHearts();
    }

    public void UISetHeartsActive(int hearts)
    {
        heartsActive = hearts;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < heartsActive)
            {
                int healthFromHeart = health - (i * 2);
                if (healthFromHeart > 1) // Full heart
                {
                    hearts[i].sprite = fullHeart;
                } else if (healthFromHeart == 1) // Half heart
                {
                    hearts[i].sprite = halfHeart;
                } else  // Empty heart
                {
                    hearts[i].sprite = emptyHeart;
                }
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
