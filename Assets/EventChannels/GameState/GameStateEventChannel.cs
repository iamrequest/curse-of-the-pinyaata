using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invoke RaiseEvent() whenever the game state is changed. 
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Game State Event Channel")]
public class GameStateEventChannel : ScriptableObject {
    // These events are respond to an invoked action
    public UnityAction<GameState> onGameStateChanged;

    // These events are responsible for invoking an action
    public UnityAction doStartGame, doStartPreGame, doStopGame;

    /// <summary>
    /// This should only be called via GameManager
    /// </summary>
    /// <param name="newGameState"></param>
    public void OnGameStateChanged(GameState newGameState) {
        // Raised for general state changes
        if (onGameStateChanged != null) {
            onGameStateChanged.Invoke(newGameState);
        }
    }

    [Button]
    [ButtonGroup("GameManagement")]
    public void DoStartGame() {
        if (doStartGame != null) {
            doStartGame.Invoke();
        }
    }

    [Button]
    [ButtonGroup("GameManagement")]
    public void DoStartPreGame() {
        if (doStartPreGame != null) {
            doStartPreGame.Invoke();
        }
    }

    [Button]
    [ButtonGroup("GameManagement")]
    public void DoStopGame() {
        if (doStopGame != null) {
            doStopGame.Invoke();
        }
    }
}
