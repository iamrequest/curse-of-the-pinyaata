using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractor : MonoBehaviour {
    [ReadOnly] [HideInEditorMode]
    public DialogueTreeController activeDialogController;
    [ReadOnly] [HideInEditorMode]
    public SubtitlesRequestInfo activeSubtitleInfo;
    [ReadOnly] [HideInEditorMode]
    public MultipleChoiceRequestInfo activeMultipleChoiceInfo;

    [HideInEditorMode]
    public DialogueTreeController debugDialogController;

    public bool startDebugDialogOnStart;

    private void Start() {
        debugDialogController = GetComponent<DialogueTreeController>();
        if (!debugDialogController) debugDialogController = FindObjectOfType<DialogueTreeController>();
        if (startDebugDialogOnStart) StartDebugDialog();
    }

    protected virtual void OnEnable() {
		//DialogueTree.OnDialogueStarted       += OnDialogueStarted;
		//DialogueTree.OnDialoguePaused        += OnDialoguePaused;
		DialogueTree.OnDialogueFinished      += OnDialogueFinished;
		DialogueTree.OnSubtitlesRequest      += OnSubtitlesRequest;
		DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
    }

    protected virtual void OnDisable() {
		//DialogueTree.OnDialogueStarted       -= OnDialogueStarted;
		//DialogueTree.OnDialoguePaused        -= OnDialoguePaused;
		DialogueTree.OnDialogueFinished      -= OnDialogueFinished;
		DialogueTree.OnSubtitlesRequest      -= OnSubtitlesRequest;
		DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
    }


    [ButtonGroup] [HideInEditorMode]
    public void StartDebugDialog() {
        if (debugDialogController == null) {
            Debug.LogError("Unable to start debug dialog: debugDialogController is null.");
            return;
        }

        if (activeDialogController != null) EndDialog();
        StartDialog(debugDialogController);
    }

    public void StartDialog(DialogueTreeController dialogController) {
        if (activeDialogController != null) return;
        //if (!dialogController.isRunning) return; // Ignore request to start dialog while dialog is already running

        activeDialogController = dialogController;
        activeDialogController.StartDialogue();
    }

    [ButtonGroup] [HideInEditorMode]
    public void EndDialog() {
        if (activeDialogController == null) return;

        if (activeDialogController.isRunning) {
            activeDialogController.StopDialogue();
        }
    }

    [ButtonGroup] [HideInEditorMode]
    public void AdvanceDialog() {
        if (activeDialogController == null) return;
        if (activeSubtitleInfo == null) return;

        activeSubtitleInfo.Continue();
    }

    [HideInEditorMode]
    [Button(ButtonStyle.CompactBox, Expanded = true)]
    public void FinalizeChoice(int choiceID) {
        if (activeDialogController == null) return;
        if (activeMultipleChoiceInfo == null) return;

        if (choiceID < 0 || choiceID >= activeMultipleChoiceInfo.options.Count) {
            Debug.LogError($"Unable to select dialog choice: selected index is out of range (value: {choiceID}");
            return;
        }

        activeMultipleChoiceInfo.SelectOption(choiceID);
    }


    private void OnSubtitlesRequest(SubtitlesRequestInfo info) {
        activeMultipleChoiceInfo = null; 
        activeSubtitleInfo = info;
    }
    private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info) {
        activeSubtitleInfo = null;
        activeMultipleChoiceInfo = info;
    }

    private void OnDialogueFinished(DialogueTree obj) {
        activeDialogController = null;
        activeMultipleChoiceInfo = null; 
        activeSubtitleInfo = null;
    }
}
