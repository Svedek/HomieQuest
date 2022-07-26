using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageHolder : MonoBehaviour {
    #region Fields ================================================================================
    [SerializeField] GameObject cageFalling;
    [SerializeField] GameObject cage;
    [SerializeField] Sprite brokenHolder;

    // Which segment is the cage holder in -> version of Cage.cs, Homie.cs
    [SerializeField] int segment;

    private int integrity = 3;
    #endregion 
    /* when hit with sword, activate child game object which has a script to make it explode and release homie when it hits ground */
    #region Unity Methods ================================================================================
	#endregion
	
    #region Public Methods ================================================================================
    public void Hit() {
        if (integrity < 0) return;
        if (--integrity == 0) { // break cage holder
            GameObject fallingCage = Instantiate(cageFalling, cage.transform.position, cage.transform.rotation, transform);
            fallingCage.GetComponent<Cage>().Initialize(segment);
            Destroy(cage);
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenHolder;
            // Particle effect + possibly pole shake
        }
        AudioManager.instance.PlaySFX("CageDamage");
    }
        #endregion

        #region Private Methods ================================================================================
        #endregion
    }
