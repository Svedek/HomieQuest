using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed, jumpForce;

    int jumps = 2;


    Rigidbody2D rb;

    Vector2 inputH = new Vector2();

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        inputH.x = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetButtonDown("Jump")) jump();
    }

    void FixedUpdate() {
        rb.AddForce(inputH, ForceMode2D.Impulse);
    }

    public void HitGround() {
        jumps = 2;
    }

    void jump()  {
        if (jumps > 0) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            --jumps;
        }
    }


}
