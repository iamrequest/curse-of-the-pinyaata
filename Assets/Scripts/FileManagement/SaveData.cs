using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public List<float> highScores;
    public bool isTutorialComplete;
    public bool isInitDialogComplete;
    public bool firstDoorBroken;
    public int numGamesPlayed;

    public SaveData() {
        highScores = new List<float>();
    }
}
