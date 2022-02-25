using NodeCanvas.BehaviourTrees;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SaveDataKey {
    IS_TUTORIAL_COMPLETE, HIGHEST_SCORE
}
public class DialogPicker : MonoBehaviour {
    public DialogActorCustom instigator;
    public BehaviourTreeOwner behaviourTreeOwner;
    public DialogueTreeController dialogTreeController;

    [FoldoutGroup("Dialog Selection Events")]
    public UnityEvent onDialogComplete;

    protected virtual void Start() {
        behaviourTreeOwner.repeat = false;
    }

    /// <summary>
    /// Use the behaviour tree to assign a DialogTree to the DialogTreeController
    /// </summary>
    [Button] [ButtonGroup]
    public void SelectDialogViaBehaviourTree(Action<bool> callback) { behaviourTreeOwner.StartBehaviour(callback); }
    public void SelectDialogViaBehaviourTree() { behaviourTreeOwner.StartBehaviour(); }

    /// <summary>
    /// Called via the behaviour tree. This assigns the DialogTree asset
    /// </summary>
    /// <param name="dialogTree"></param>
    public void SelectDialog(DialogueTree dialogTree) {
        dialogTreeController.behaviour = dialogTree;
    }

    [Button][ButtonGroup]
    public void StartSelectedDialog() {
        ActiveDialogListener.Instance.populateActorCallback(dialogTreeController);
        if (instigator) {
            dialogTreeController.StartDialogue(instigator, OnDialogComplete);
        } else {
            dialogTreeController.StartDialogue(OnDialogComplete);
        }
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
    }

    protected virtual void OnDialogComplete(bool wasDialogFinished) {
        // I should pass the bool parameter through the UnityEvent, but I can't serialize the UnityEvent field if I do
        //  The workaround is probably to define custom classes (see: DamageEvents), but I don't need that bool parameter just yet.
        onDialogComplete.Invoke();
    }


    public bool GetProgressionBool(SaveDataKey saveDataKey) {
        switch(saveDataKey) {
            // TODO: Should reading save data be extracted into a separate class?
            case SaveDataKey.IS_TUTORIAL_COMPLETE: return SaveManager.Instance.saveData.isTutorialComplete;
            default:
                Debug.LogWarning($"Unexpected save data key found when getting bool progression value: {saveDataKey}");
                return false;
        }
    }
    public float GetProgressionFloat(SaveDataKey saveDataKey) {
        switch(saveDataKey) {
            case SaveDataKey.HIGHEST_SCORE: return SaveManager.Instance.saveData.highScores[0];
            default:
                Debug.LogWarning($"Unexpected save data key found while getting float progression value: {saveDataKey}");
                return 0f;
        }
    }
}
