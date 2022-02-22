using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    ACTIVE, FINISHED
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
        gameStateEventChannel.doStopGame += EndGame;
    }
    private void OnDisable() {
        gameStateEventChannel.doStartGame -= StartGame;
        gameStateEventChannel.doStopGame -= EndGame;
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

        gameTimerCoroutine = StartCoroutine(EndGameAfterDelay());

        // Reset the score
        ScoreManager.Instance.ResetScore();
    }

    [Button] [HideInEditorMode]
    [ButtonGroup("Game Start Stop")]
    public void EndGame() {
        if (gameState != GameState.ACTIVE) {
            return;
        }

        gameState = GameState.FINISHED;
        gameStateEventChannel.OnGameStateChanged(gameState);

        // Stop the game timer
        if (gameTimerCoroutine != null) {
            StopCoroutine(gameTimerCoroutine);
            gameTimerCoroutine = null;
        }

        // Attempt to add this score to the high score board
        ScoreManager.Instance.CompareScoreToHighScore();
    }
    private IEnumerator EndGameAfterDelay() {
        gameDurationCurrent = 0f;

        // Wait until the game is over
        while (gameDurationCurrent < gameDurationTotal) {
            gameDurationCurrent += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Optional: Snap the elapsed time to max time here
        EndGame();
    }
}
