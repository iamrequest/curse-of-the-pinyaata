using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerRenderer : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public TextMeshProUGUI timerTextField;

    private void Start() {
        UpdateUI();
    }
    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += UpdateUIOnGameStateChange;
    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= UpdateUIOnGameStateChange;
    }

    private void UpdateUIOnGameStateChange(GameState newGameState) {
        UpdateUI();
    }

    private void Update() {
        if (GameManager.Instance.gameState == GameState.ACTIVE) {
            UpdateUI();
        }
    }

    private void UpdateUI() {
        switch (GameManager.Instance.gameState) {
            case GameState.NOT_STARTED:
                timerTextField.text = "Time: --:--";
                break;
            case GameState.ACTIVE:
                double minutes = Math.Floor(GameManager.Instance.gameDurationCurrent / 60f);
                double seconds = Math.Floor(GameManager.Instance.gameDurationCurrent % 60f);
                timerTextField.text = $"Time: {minutes.ToString("00")}:{seconds.ToString("00")}";
                break;
            case GameState.FINISHED:
                timerTextField.text = "Time: 00:00";
                break;
        }
    }

    //[Button]
    //public void DebugRenderUI(float t) {
    //    double minutes = Math.Floor(t / 60f);
    //    double seconds = Math.Floor(t % 60f);
    //    timerTextField.text = $"Time: {minutes.ToString("00")}:{seconds.ToString("00")}";
    //}
}
