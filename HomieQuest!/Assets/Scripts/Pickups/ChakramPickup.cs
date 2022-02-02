using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChakramPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) // If collision is player
        {
            player.UnlockChakram();
            Destroy(gameObject);
        }
    }
}