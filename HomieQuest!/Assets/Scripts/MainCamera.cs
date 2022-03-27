using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour {
    #region Fields ================================================================================
    Camera cam;
    private float size = 9f;
    [SerializeField] Text text;
	#endregion
	
    #region Unity Methods ================================================================================
    void Awake() {
        cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = size;
    }

    void Update() {
        TestFov();
    }
    #endregion

    #region Public Methods ================================================================================
    #endregion

    #region Private Methods ================================================================================
    private void TestFov() {
        if (Input.GetKeyDown(KeyCode.I)) {
            cam.orthographicSize += 0.1f;
            if (text != null) text.text = "Size: " + cam.orthographicSize;
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            cam.orthographicSize -= 0.1f;
            if (text != null) text.text = "Size: " + cam.orthographicSize;
        }
    }
	#endregion
}
