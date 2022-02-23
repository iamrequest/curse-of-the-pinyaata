using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class HighScoreRenderer : MonoBehaviour {
    public TextMeshProUGUI highScoreText;

    private void Start() {
        try {
            SaveManager.Instance.LoadData();
            UpdateUI();
        } catch (Exception e) {
            Debug.LogError("Unable to read high scores from disk: " + e.Message);
            RenderBlankUI();
        }
    }

    [Button]
    private void UpdateUI() {
        highScoreText.text = "";
        for (int i = 0; i < ScoreManager.Instance.maxNumHighScores; i++) {
            highScoreText.text += $"{(i + 1).ToString("00")}: {ScoreManager.Instance.highScores[i].ToString("000000")}\n";
        }
    }

    [Button]
    private void RenderBlankUI() {
        highScoreText.text = "";
        for (int i = 0; i < ScoreManager.Instance.maxNumHighScores; i++) {
            highScoreText.text += $"{(i + 1).ToString("00")}: 000000\n";
        }
    }
}
