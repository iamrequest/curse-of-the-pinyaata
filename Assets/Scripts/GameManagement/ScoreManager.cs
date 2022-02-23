using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;

    public float currentScore;
    // TODO: Depending on time constraints & UI options, consider making this a separate object with player name and timestamp
    public int maxNumHighScores = 10;
    public List<float> highScores;
    public UnityEvent onScoreUpdated;

    public static ScoreManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple ScoreManager components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void Start() {
        // Init high scores list
        for (int i = 0; i < maxNumHighScores; i++) { highScores.Add(0); }
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.ACTIVE:
                ResetScore();
                break;
            case GameState.FINISHED:
                CompareScoreToHighScore();
                break;
        }
    }


    public void AddScore(float addedScore) {
        if (GameManager.Instance.gameState == GameState.ACTIVE) {
            currentScore += addedScore;
            onScoreUpdated.Invoke();
        }
    }

    [Button("Add 10 points")] [HideInEditorMode]
    [ButtonGroup("Debug")]
    private void DebugAddScore() {
        AddScore(10f);
    }

    [Button] [HideInEditorMode]
    [ButtonGroup("Debug")]
    public void ResetScore() {
        currentScore = 0;
        onScoreUpdated.Invoke();
    }

    /// <summary>
    /// Given currentScore, add it to the list of high scores if it beats any of the current scores
    /// </summary>
    [Button] [HideInEditorMode]
    [ButtonGroup("Debug")]
    public void CompareScoreToHighScore() {
        for(int i = 0; i < maxNumHighScores; i++) {
            if (currentScore > highScores[i]) {
                highScores.RemoveAt(highScores.Count - 1);
                highScores.Insert(i, currentScore);

                // TODO: Call score manager event channel here
                SaveManager.Instance.SaveData();
                return;
            }
        }
    }
}
