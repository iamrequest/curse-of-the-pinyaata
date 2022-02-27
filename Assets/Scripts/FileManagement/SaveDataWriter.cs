using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to SaveManager.UpdateSaveData(), used for calling via unity event
/// Big hack to make this work with unity events
/// </summary>
[RequireComponent(typeof(Damageable))]
public class SaveDataWriter : MonoBehaviour {
    private Damageable damageable;
    public SaveDataKeys key;
    public bool value = true;

    private void Awake() {
        damageable = GetComponent<Damageable>();
    }
    private void OnEnable() {
        damageable.onHealthDepleted.AddListener(OnDestroyed);
    }
    private void OnDisable() {
        damageable.onHealthDepleted.AddListener(OnDestroyed);
    }

    private void OnDestroyed(Damageable arg0, Vector3 arg1) {
        SaveManager.Instance.UpdateSaveData(key, value);
    }
}
