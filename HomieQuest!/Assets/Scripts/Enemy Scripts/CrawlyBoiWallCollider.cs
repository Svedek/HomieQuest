using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlyBoiWallCollider : MonoBehaviour {
    private CrawlyBoiController crawlyBoi;
    private void Start() {
        crawlyBoi = gameObject.GetComponentInParent<CrawlyBoiController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer >= 7 && collision.gameObject.layer <= 9) { // Enemy layer, Terrain layer, Bouncable Hazard layer
            crawlyBoi.HitWall(transform.localPosition.x);
            transform.localPosition = new Vector2(transform.localPosition.x * -1, transform.localPosition.y);
        }
    }
}