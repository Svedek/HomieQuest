using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour {
    private int damage = 1;
    [SerializeField] private float knockback = 30f;

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) { // If collision is player
            // Find the average position of coliding points
            ContactPoint2D[] contactList = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactList);
            Vector2 point = Vector2.zero;
            foreach (ContactPoint2D contact in contactList) { 
                point += contact.point;
            }
            point = point / collision.contactCount;

            // Find direction of knockback and apply damage to player
            Vector2 KBDir = ((Vector2)collision.transform.position - point).normalized;
            player.HitPlayer(damage, KBDir, knockback);
        }
    }
}
