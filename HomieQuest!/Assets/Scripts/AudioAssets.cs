using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAssets {
    #region Fields ================================================================================
    private static AudioAssets _instance;



    // Settings
    float sfxVol;
    float musicVol;

    /// SFX
    // Player
    public Sound playerSwing { get; private set; }

    // Enemy

    /// Music

    #endregion

    #region Public Methods ================================================================================
    public static AudioAssets Instance {
        get {
            if (_instance == null) {
                _instance = new AudioAssets();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    /// Sound Returns
    // SFX
    // Music

    #endregion

    #region Private Methods ================================================================================
    // Set all referances
    private void Initialize() {
        
    }
    #endregion
}
