using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagementInteractor : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public SceneManagerEventChannel sceneManagerEventChannel;

    // TODO: Fetch the appropriate pre/post-game dialog from the progression manager, once that's implemented
    public DialogueTreeController dialogTreeController;
    public DialogActorCustom dialogActor;
    public DialogueTree preGameDialog;
    public bool startPreGameDialogAfterDelay;
    public float preGameDialogDelay;

    public DialogueTree postGameDialog;
    public bool startPostGameDialogAfterDelay;
    public float postGameDialogDelay;
    public float postGameSceneLoadDelay;

    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    // Called from the dialog tree
    public void StartGame() {
        gameStateEventChannel.DoStartGame();
    }

    public void Start() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;

        // Start pre-game dialog
        if (preGameDialog) {
            if (startPreGameDialogAfterDelay) {
                StartCoroutine(StartPreGameDialogAfterDelay());
            } else {
                dialogTreeController.StartDialogue(preGameDialog, dialogActor, null);
                ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
            }
        }
    }

    private IEnumerator LoadTitleSceneAfterDelay() {
        yield return new WaitForSeconds(postGameSceneLoadDelay);
        sceneManagerEventChannel.LoadTitleScene();
    }

    private IEnumerator StartPreGameDialogAfterDelay() {
        yield return new WaitForSeconds(preGameDialogDelay);
        dialogTreeController.StartDialogue(preGameDialog, dialogActor, null);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
        ActiveDialogListener.Instance.onDialogFinished += OnPreDialogFinished;
    }

    private void OnPreDialogFinished(DialogueTreeController dialogueTreeController) {
        ActiveDialogListener.Instance.onDialogFinished -= OnPreDialogFinished;

        // This may need to be configurable
        gameStateEventChannel.DoStartGame();
    }

    private IEnumerator StartPostGameDialogAfterDelay() {
        yield return new WaitForSeconds(postGameDialogDelay);
        dialogTreeController.StartDialogue(postGameDialog, dialogActor, null);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
        ActiveDialogListener.Instance.onDialogFinished += OnPostGameDialogComplete;
    }


    private void OnPostGameDialogComplete(DialogueTreeController dialogTreeController) {
        ActiveDialogListener.Instance.onDialogFinished -= OnPostGameDialogComplete;
        StartCoroutine(LoadTitleSceneAfterDelay());
    }

    private void OnGameStateChanged(GameState newGameState) {
        // At the end of the game, start the post-game dialog
        if (newGameState == GameState.FINISHED) {
            if (postGameDialog) {
                if (startPreGameDialogAfterDelay) {
                    StartCoroutine(StartPostGameDialogAfterDelay());
                } else {
                    dialogTreeController.StartDialogue(postGameDialog, dialogActor, null);
                    ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
                    ActiveDialogListener.Instance.onDialogFinished += OnPostGameDialogComplete;
                }
            } else {
                // If there's no dialog, just return to the main scene
                StartCoroutine(LoadTitleSceneAfterDelay());
            }
        }
    }
}
