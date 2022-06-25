using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingChecker : MonoBehaviour {
    [SerializeField] private Vector3 XOffset, YOffset;
    [SerializeField] private Vector3 HSize, VSize;
    [SerializeField] private int swingTime = 3;

    private PlayerController player;
    private Collider2D collider2d;
    private int swingTimer = 0, position = 0; // pos - 0 is right, 1 is up, 2 is left, 3 is down

    // Start is called before the first frame update
    void Start() {
        player = gameObject.GetComponentInParent<PlayerController>();
        collider2d = gameObject.GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate() {
        if (swingTimer > 0) {
            if (--swingTimer <= 0) { // Decrement + If was final frame of swing
                EndSwing();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyLifeform enemy = collision.gameObject.GetComponent<EnemyLifeform>();
        if (enemy != null || collision.gameObject.layer == 9) {
            player.SwingConnect(enemy, position);
        } else {
            CageHolder holder = collision.gameObject.GetComponent<CageHolder>();
            if(holder != null) {
                holder.Hit();
            }
        }
    }

    public void SwingUp() {
        // Set up collision area
        transform.localPosition = YOffset;
        transform.localScale = VSize;

        position = 1;

        StartSwing();
    }
    public void SwingDown() {
        // Set up collision area
        transform.localPosition = YOffset * -1;
        transform.localScale = VSize;

        position = 3;

        StartSwing();
    }
    public void SwingRight() {
        // Set up collision area
        transform.localPosition = XOffset;
        transform.localScale = HSize;

        position = 0;

        StartSwing();
    }
    public void SwingLeft() {
        // Set up collision area
        transform.localPosition = XOffset * -1;
        transform.localScale = HSize;

        position = 2;

        StartSwing();
    }

    private void StartSwing() {
        swingTimer = swingTime;
        collider2d.enabled = true;
    }

    private void EndSwing() {
        collider2d.enabled = false;
    }
}