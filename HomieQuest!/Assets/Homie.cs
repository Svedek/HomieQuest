using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homie : MonoBehaviour {
    #region Fields ================================================================================
    [SerializeField] float upForce;
    [SerializeField] Sprite[] homies;

    [SerializeField] Text dialogue;
    #endregion

    void Start() {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        AudioManager.instance.PlaySFX("PlayerHitGround");
        dialogue.enabled = true;
    }

    // called immedeately upon creation
    public void Initialize(int ver) {
        var index = ver - 1;
        GetComponent<SpriteRenderer>().sprite = homies[index];
        dialogue.text = text[index];
        dialogue.color = color[index];
    }

    private static string[] text = {
        "Thanks for saving me!\nHere is my chakram. You can bounce off it mid-air to reach new heights!",
        "Yo! Whats up homie!\nHeres my dash module, good luck rescuing the rest of the homies!"
    };

    private static Color[] color = {
        new Color(.17f, .74f, .37f),
        new Color(.77f, 0f, 0f),
    };
}
