using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;

    public HVRPlayerController playerController { get; private set; }
    private Vector3 spawnPosition, spawnDirection;

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
        spawnDirection = playerController.transform.forward;
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void Start() {
        ReturnToStartTransform();
    }

    [Button]
    public void ReturnToStartTransform() {
        playerController.Teleporter.Teleport(spawnPosition, spawnDirection);
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.FINISHED:
                ReturnToStartTransform();
                break;
        }
    }
}
