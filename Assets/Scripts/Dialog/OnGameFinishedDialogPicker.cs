using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using UnityEngine.Events;

/// <summary>
/// This class starts pre-game and post-game dialog with the Pinyaata, and takes care of post-dialog tasks.
/// "Dialog Picker" is a name that needs refactoring - this is more of a Manager class.
/// </summary>
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
        //dialogTreeController.StartDialogue(actor, OnDialogFinished);
        dialogTreeController.StartDialogue(OnDialogFinished);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
    }

    private void OnDialogFinished(bool wasDialogSuccessful) {
        switch (GameManager.Instance.gameState) {
            case GameState.PREGAME:
                gameStateEventChannel.DoStartGame();
                break;
            case GameState.FINISHED:
                IncrementGamesPlayedCounter();
                sceneManagerEventChannel.LoadTitleScene();
                break;
        }
    }

    public void UpdateSaveData(SaveDataKeys key, bool value) {
        SaveManager.Instance.UpdateSaveData(key, value);
    }
    public void UpdateSaveData(SaveDataKeys key, int value) {
        SaveManager.Instance.UpdateSaveData(key, value);
    }

    public bool CheckSaveDataBool(SaveDataKeys key) {
        return SaveManager.Instance.GetSaveDataValueBool(key);
    }
    public int CheckSaveDataInt(SaveDataKeys key) {
        return SaveManager.Instance.GetSaveDataValueInt(key);
    }
    public void IncrementGamesPlayedCounter() {
        SaveManager.Instance.UpdateSaveData(SaveDataKeys.numGamesPlayed, SaveManager.Instance.saveData.numGamesPlayed + 1);
    }
}
