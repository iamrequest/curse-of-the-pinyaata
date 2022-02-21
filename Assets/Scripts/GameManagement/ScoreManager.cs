using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public int currentScore;
    // TODO: Depending on time constraints & UI options, consider making this a separate object with player name and timestamp
    public int maxNumHighScores = 10;
    public List<int> highScores;

    public static ScoreManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple ScoreManager components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void Start() {
        // Init high scores list
        for (int i = 0; i < maxNumHighScores; i++) { highScores.Add(0); }
    }

    public void AddScore(int addedScore) {
        if (GameManager.Instance.gameState == GameState.ACTIVE) {
            currentScore += addedScore;
        }
    }

    [Button("Add 10 points")] [HideInEditorMode]
    [ButtonGroup("Debug")]
    private void DebugAddScore() {
        AddScore(10);
    }

    [Button] [HideInEditorMode]
    [ButtonGroup("Debug")]
    public void ResetScore() {
        currentScore = 0;
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
                return;
            }
        }
    }
}
