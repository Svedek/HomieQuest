using System.Collections;
using System.Collections.Generic;

public class HintManager {

    public static HintManager Instance { 
        get { 
            if (instance == null) instance = new HintManager();
            return instance;
        } 
    }
    private static HintManager instance;

    public bool HintsEnabled { get; private set; } = true;

    public void SetHintVisability(bool visability) {
        if (visability == HintsEnabled) return;
        HintsEnabled = visability;
        ChangeHintVisibility();
    }

    public void EnableHints() {
        HintsEnabled = true;
        ChangeHintVisibility();
    }

    public void DisableHints() {
        HintsEnabled = false;
        ChangeHintVisibility();
    }

    private void ChangeHintVisibility() {
        OnHintChange?.Invoke(HintsEnabled);
    }



    public delegate void OnHintVisibilityChange(bool visibility);
    public event OnHintVisibilityChange OnHintChange;




}
