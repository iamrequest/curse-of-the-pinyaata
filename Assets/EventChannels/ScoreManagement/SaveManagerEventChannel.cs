using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Save Manager Event Channel")]
public class SaveManagerEventChannel : ScriptableObject {
    // These are called in response to save/load/delete operations
    public UnityAction<SaveData> onSave, onLoad, onDelete;

    public void OnSave(SaveData saveData) {
        // Raised for general state changes
        if (onSave != null) {
            onSave.Invoke(saveData);
        }
    }
    public void OnLoad(SaveData loadedData) {
        // Raised for general state changes
        if (onLoad != null) {
            onLoad.Invoke(loadedData);
        }
    }

    public void OnDelete(SaveData blankSaveData) {
        // Raised for general state changes
        if (onDelete != null) {
            onDelete.Invoke(blankSaveData);
        }
    }
}
