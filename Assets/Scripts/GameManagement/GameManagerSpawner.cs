using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour {
    public GameObject gameManagerPrefab;
    [HideInInspector]
    public GameObject gameManagerInstance;

    private void Awake() {
        DontDestroyOnLoad(gameObject); 
        if (gameManagerInstance == null) {
            gameManagerInstance = Instantiate(gameManagerPrefab, transform);
        }
    }
}
