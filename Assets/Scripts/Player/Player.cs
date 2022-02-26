using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;

    public HVRPlayerController playerController { get; private set; }
    public HVRSocket chestSocket;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public static Player Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }

        playerController = GetComponentInChildren<HVRPlayerController>();
        // animator = GetComponentInChildren<Animator>();
        // animHashVisionFade = Animator.StringToHash("visionFade");
        spawnPosition = playerController.transform.position;
        spawnRotation = playerController.transform.rotation;
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void Start() {
        ReturnToStartTransform();
        StartCoroutine(DisableBatSocketAfterDelay());
    }

    private IEnumerator DisableBatSocketAfterDelay() {
        // Waiting until the end of the frame to let the bat socket itself on start()
        yield return new WaitForEndOfFrame();
        chestSocket.gameObject.SetActive(false);
    }

    public void ReturnToStartTransform() {
        playerController.transform.position = spawnPosition;
        playerController.transform.rotation = spawnRotation;
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.ACTIVE:
                chestSocket.gameObject.SetActive(true);
                break;
            case GameState.FINISHED:
                StartCoroutine(DisableBatSocketAfterDelay());
                ReturnToStartTransform();
                break;
        }
    }
}
