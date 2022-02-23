using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    public static SaveManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple SaveManager components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void SaveData() {
        SaveData sd = new SaveData(ScoreManager.Instance.highScores);
        SaveSystem.SavePlayerData(sd);
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void LoadData() {
        SaveData sd = SaveSystem.LoadPlayerData();

        // TODO: This can lead to indexing errors
        ScoreManager.Instance.highScores = sd.highScores;
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void DeleteData() {
        SaveSystem.DeletePlayerData();
    }
}
