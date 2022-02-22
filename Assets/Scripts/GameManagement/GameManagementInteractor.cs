using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagementInteractor : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public ActiveDialogEventChannel activeDialogEventChannel;
    public SceneManagerEventChannel sceneManagerEventChannel;

    // TODO: Fetch the appropriate pre/post-game dialog from the progression manager, once that's implemented
    public DialogueTreeController preGameDialog;
    public bool startPreGameDialogAfterDelay;
    public float preGameDialogDelay;

    public DialogueTreeController postGameDialog;
    public bool startPostGameDialogAfterDelay;
    public float postGameDialogDelay;
    public float postGameSceneLoadDelay;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    // Called from the dialog tree
    public void StartGame() {
        gameStateEventChannel.DoStartGame();
    }

    public void Start() {
        // Start pre-game dialog
        if (preGameDialog) {
            if (startPreGameDialogAfterDelay) {
                StartCoroutine(StartPreGameDialogAfterDelay());
            } else {
                activeDialogEventChannel.StartDialog(preGameDialog);
            }
        }
    }

    private IEnumerator LoadTitleSceneAfterDelay() {
        yield return new WaitForSeconds(postGameSceneLoadDelay);
        sceneManagerEventChannel.LoadTitleScene();
    }

    private IEnumerator StartPreGameDialogAfterDelay() {
        yield return new WaitForSeconds(preGameDialogDelay);
        activeDialogEventChannel.StartDialog(preGameDialog);

        // This may need to be configurable
        gameStateEventChannel.DoStartGame();
    }

    private IEnumerator StartPostGameDialogAfterDelay() {
        yield return new WaitForSeconds(postGameDialogDelay);
        activeDialogEventChannel.StartDialog(postGameDialog);
        activeDialogEventChannel.onDialogFinished += OnPostGameDialogComplete;
    }


    private void OnPostGameDialogComplete() {
        activeDialogEventChannel.onDialogFinished -= OnPostGameDialogComplete;
        StartCoroutine(LoadTitleSceneAfterDelay());
    }

    private void OnGameStateChanged(GameState newGameState) {
        // At the end of the game, start the post-game dialog
        if (newGameState == GameState.FINISHED) {
            if (postGameDialog) {
                if (startPreGameDialogAfterDelay) {
                    StartCoroutine(StartPostGameDialogAfterDelay());
                } else {
                    activeDialogEventChannel.StartDialog(postGameDialog);
                    activeDialogEventChannel.onDialogFinished += OnPostGameDialogComplete;
                }
            } else {
                // If there's no dialog, just return to the main scene
                StartCoroutine(LoadTitleSceneAfterDelay());
            }
        }
    }
}
