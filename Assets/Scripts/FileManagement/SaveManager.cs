using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour {
    public SaveManagerEventChannel saveManagerEventChannel;
    private SaveData m_saveData;
    public SaveData saveData {
        get {
            if (m_saveData == null) {
                // If we fail to load the data, this will return an empty SaveData instance
                LoadData();

                if (m_saveData == null) {
                    m_saveData = new SaveData();
                }
            }

            return m_saveData;
        }  set {
            m_saveData = value;
        }
    }

    public static SaveManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple SaveManager components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void Start() {
        LoadData();
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void SaveData() {
        SaveSystem.SavePlayerData(saveData);
        saveManagerEventChannel.OnSave(saveData); 
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void LoadData() {
        try {
            m_saveData = SaveSystem.LoadPlayerData();
            if (m_saveData == null) {
                m_saveData = new SaveData();
            }
        } catch (Exception e) {
            Debug.LogWarning($"Unable to load save data. Creating new save data: {e.Message}");
            m_saveData = new SaveData();
        }

        saveManagerEventChannel.OnLoad(m_saveData); 
    }

    [Button] [ButtonGroup("Save")]
    [HideInEditorMode]
    public void DeleteData() {
        SaveSystem.DeletePlayerData();
        saveData = new SaveData();
        saveManagerEventChannel.OnDelete(saveData); 
    }
}
