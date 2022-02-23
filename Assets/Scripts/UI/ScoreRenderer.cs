using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ScoreRenderer : MonoBehaviour {
    public TextMeshProUGUI scoreTextField;

    private void OnEnable() {
        // Wait for singleton reference to populate
        StartCoroutine(SubscribeToEventsAfterDelay());
    }
    private IEnumerator SubscribeToEventsAfterDelay() {
        yield return new WaitForEndOfFrame();
        ScoreManager.Instance.onScoreUpdated.AddListener(UpdateUI);
    }
    private void OnDisable() {
        ScoreManager.Instance.onScoreUpdated.RemoveListener(UpdateUI);
    }

    private void Start() {
        UpdateUI();
    }

    private void UpdateUI() {
        scoreTextField.text = $"Score:{ScoreManager.Instance.currentScore.ToString("000000")}";
    }
}
