using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour {
    void Awake() {
        gameObject.SetActive(HintManager.Instance.HintsEnabled);
        HintManager.Instance.OnHintChange += OnHintVisibilityChange;
    }
    private void OnDestroy() {
        HintManager.Instance.OnHintChange -= OnHintVisibilityChange;
    }

    private void OnHintVisibilityChange(bool visible) {
        gameObject.SetActive(visible);
    }
}
