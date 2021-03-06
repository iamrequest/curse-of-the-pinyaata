using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    NOT_STARTED, PREGAME, ACTIVE, FINISHED, STOPPED
}
public class GameManager : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public GameState gameState { get; private set; }
    public float gameDurationTotal, gameDurationCurrent;

    private Coroutine gameTimerCoroutine;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple GameManager components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void OnEnable() {
        gameStateEventChannel.doStartGame += StartGame;
        gameStateEventChannel.doStartPreGame += StartPreGame;
        gameStateEventChannel.doStopGame += EndGame;
    }
    private void OnDisable() {
        gameStateEventChannel.doStartGame -= StartGame;
        gameStateEventChannel.doStartPreGame -= StartPreGame;
        gameStateEventChannel.doStopGame -= EndGame;
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }


    [Button] [HideInEditorMode]
    [ButtonGroup("Game Start Stop")]
    public void StartGame() {
        if (gameState == GameState.ACTIVE) {
            return;
        }

        // Start the game, and game timer
        gameState = GameState.ACTIVE;
        gameStateEventChannel.OnGameStateChanged(gameState);
        Debug.Log("Starting game");

        gameTimerCoroutine = StartCoroutine(EndGameAfterDelay());
    }

    [Button] [HideInEditorMode]
    [ButtonGroup("Game Start Stop")]
    private void StartPreGame() {
        gameState = GameState.PREGAME;
        gameStateEventChannel.OnGameStateChanged(gameState);
    }


    [Button] [HideInEditorMode]
    [ButtonGroup("Game Start Stop")]
    public void EndGame() {
        if (gameState != GameState.ACTIVE && gameState != GameState.STOPPED) {
            return;
        }

        gameState = GameState.FINISHED;
        gameStateEventChannel.OnGameStateChanged(gameState);
        Debug.Log("Finishing game");

        // Stop the game timer
        if (gameTimerCoroutine != null) {
            StopCoroutine(gameTimerCoroutine);
            gameTimerCoroutine = null;
        }
    }


    // This is a special case used for the final dialog with the pinyaata
    [Button] [HideInEditorMode]
    [ButtonGroup("Game Start Stop")]
    public void StopGame() {
        // Stop the game timer
        if (gameTimerCoroutine != null) {
            StopCoroutine(gameTimerCoroutine);
            gameTimerCoroutine = null;
        }
        gameState = GameState.STOPPED;
    }

    private IEnumerator EndGameAfterDelay() {
        gameDurationCurrent = gameDurationTotal;

        // Wait until the game is over
        while (gameDurationCurrent > 0f) {
            gameDurationCurrent -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Snap the elapsed time to max time here
        gameDurationCurrent = 0f;
        EndGame();
    }
}
