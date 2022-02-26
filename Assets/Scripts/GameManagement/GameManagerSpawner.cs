using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour {
    public static GameManagerSpawner Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            if (gameManagerInstance == null) {
                gameManagerInstance = Instantiate(gameManagerPrefab, transform);
            }
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(gameObject);
        }

    }

    public GameObject gameManagerPrefab;
    [HideInInspector]
    public GameObject gameManagerInstance;
}
