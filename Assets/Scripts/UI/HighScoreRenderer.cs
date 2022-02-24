using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class HighScoreRenderer : MonoBehaviour {
    public SaveManagerEventChannel saveManagerEventChannel;
    public TextMeshProUGUI highScoreText;


    private void OnEnable() {
        saveManagerEventChannel.onSave += OnSaveDataChanged;
        saveManagerEventChannel.onLoad += OnSaveDataChanged;
        saveManagerEventChannel.onDelete += OnSaveDataChanged;
    }
    private void OnDisable() {
        saveManagerEventChannel.onSave -= OnSaveDataChanged;
        saveManagerEventChannel.onLoad -= OnSaveDataChanged;
        saveManagerEventChannel.onDelete -= OnSaveDataChanged;
    }

    private void Start() {
        UpdateUI();
    }
    private void OnSaveDataChanged(SaveData updatedSaveData) {
        UpdateUI();
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
