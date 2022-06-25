using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour {
    #region Fields ================================================================================
    [SerializeField] private GameObject homie;
    [SerializeField] private Sprite[] cages;
    [SerializeField] private Sprite[] cageBroken;

    [SerializeField] GameObject[] drops;
    [SerializeField] float dropYoffset;

    private Rigidbody2D rb;
    private int version = -1;
	#endregion
	
    #region Unity Methods ================================================================================
    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 8) {
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<SpriteRenderer>().sprite = cageBroken[version-1];
            AudioManager.instance.PlaySFX("CageBreak");
            // Instanciate homie and powerup
            GameObject homes = Instantiate(homie, transform.position, transform.rotation, transform);
            homes.GetComponent<Homie>().Initialize(version);

            Instantiate(drops[version-1], transform.position + (Vector3.up * dropYoffset), transform.rotation, transform);
        }
    }
    #endregion

    // called immedeately upon creation
    public void Initialize(int ver) {
        GetComponent<SpriteRenderer>().sprite = cages[ver - 1];
        version = ver;
    }
}
