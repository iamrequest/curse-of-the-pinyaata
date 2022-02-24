using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public SaveManagerEventChannel saveManagerEventChannel;

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
        saveManagerEventChannel.onLoad += ReadHighScoreFromSave;
        saveManagerEventChannel.onDelete += ReadHighScoreFromSave;
    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
        saveManagerEventChannel.onLoad -= ReadHighScoreFromSave;
        saveManagerEventChannel.onDelete -= ReadHighScoreFromSave;
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

    private void ReadHighScoreFromSave(SaveData saveData) {
        highScores = saveData.highScores;

        // If the save data has too few records (ie: less than the max number that we want to store), then add them now
        //  Note: Need to cache high score count, to avoid for loop indexing errors (loop definition changes during for loop)
        int numHighScoresInSave = saveData.highScores.Count;
        for (int i = 0; i < maxNumHighScores - numHighScoresInSave; i++) highScores.Add(0f);

        // If the save data has too many records, truncate the end of the high scores list
        if (highScores.Count > maxNumHighScores) {
            highScores.RemoveRange(maxNumHighScores, highScores.Count - maxNumHighScores);
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

                // TODO: Call score manager event channel here, if that's needed

                // Save scores to disk
                SaveManager.Instance.saveData.highScores = highScores;
                SaveManager.Instance.SaveData();
                return;
            }
        }
    }
}
