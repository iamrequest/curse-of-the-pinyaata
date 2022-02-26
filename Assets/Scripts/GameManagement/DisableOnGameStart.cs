using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameStart : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.PREGAME:
            case GameState.ACTIVE:
                gameObject.SetActive(false);
                break;
        }
    }
}
