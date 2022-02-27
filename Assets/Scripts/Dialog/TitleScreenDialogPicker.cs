using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class starts on start dialog with the NPCs, and takes care of post-dialog tasks.
/// "Dialog Picker" is a name that needs refactoring - this is more of a Manager class.
///
/// This class is the refactored version of OnStartDialogPicker, which I don't want to remove just yet (it's late)
/// </summary>
public class TitleScreenDialogPicker : MonoBehaviour {
    public DialogueTreeController dialogTreeController;
    public UIPhysicsButton tutorialButton, startButton;

    private void OnEnable() {
        StartCoroutine(AfterInit());
    }
    private IEnumerator AfterInit() {
        yield return new WaitForEndOfFrame();
        BGMManager.Instance.PlaySong(0);
    }

    private void Start() {
        StartCoroutine(StartDialogAfterDelay());
    }


    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(PingShaderManager.Instance.blindfoldTransitionDuration);
        dialogTreeController.StartDialogue(OnDialogFinished);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
    }

    private void OnDialogFinished(bool wasDialogFinishedSuccessfully) {
        tutorialButton.EnableButton();
        if (SaveManager.Instance.saveData.isTutorialComplete) {
            startButton.EnableButton();
        }
    }

    public void UpdateSaveData(SaveDataKeys key, bool value) {
        SaveManager.Instance.UpdateSaveData(key, value);
    }

    public bool CheckSaveDataBool(SaveDataKeys key) {
        return SaveManager.Instance.GetSaveDataValueBool(key);
    }
    public int CheckSaveDataInt(SaveDataKeys key) {
        return SaveManager.Instance.GetSaveDataValueInt(key);
    }
}
