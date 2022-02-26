using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class enables/disables the renderer, depending on the game state
/// </summary>
public class PinataRendererController : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public Renderer m_renderer;
    public bool initVisibility, gameVisibility, postGameVisibility;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }
    private void Start() {
        m_renderer.enabled = initVisibility;
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.ACTIVE:
                m_renderer.enabled = gameVisibility;
                break;
            case GameState.FINISHED:
                m_renderer.enabled = postGameVisibility;
                break;
        }
    }
}
