using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class OnGameFinishedDialogPicker : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public SceneManagerEventChannel sceneManagerEventChannel;
    public GameState cachedGameState;

    public DialogActorCustom actor;
    public DialogueTreeController dialogTreeController;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState) {
        cachedGameState = newGameState;
        switch (GameManager.Instance.gameState) {
            case GameState.PREGAME:
            case GameState.FINISHED:
                // Pick some dialog to say, and start it as soon as it's picked
                StartCoroutine(StartDialogAfterDelay());
                break;
        }
    }

    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(PingShaderManager.Instance.blindfoldTransitionDuration);
        dialogTreeController.StartDialogue(actor, OnDialogFinished);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
    }

    private void OnDialogFinished(bool wasDialogSuccessful) {
        switch (GameManager.Instance.gameState) {
            case GameState.PREGAME:
                gameStateEventChannel.DoStartGame();
                break;
            case GameState.FINISHED:
                sceneManagerEventChannel.LoadTitleScene();
                break;
        }
    }
}
