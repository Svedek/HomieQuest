using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homie : MonoBehaviour {
    #region Fields ================================================================================
    [SerializeField] float upForce;
    [SerializeField] Sprite[] homies;
    #endregion

    void Start() {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up*upForce,ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        AudioManager.instance.PlaySFX("PlayerHitGround");
    }

    // called immedeately upon creation
    public void Initialize(int ver) {
        GetComponent<SpriteRenderer>().sprite = homies[ver - 1];
    }
}
