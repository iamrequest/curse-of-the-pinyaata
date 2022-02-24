using NodeCanvas.BehaviourTrees;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaveDataKey {
    IS_TUTORIAL_COMPLETE, HIGHEST_SCORE
}
public class DialogPicker : MonoBehaviour {
    public DialogActorCustom actor;
    public BehaviourTreeOwner behaviourTreeOwner;
    public DialogueTreeController dialogTreeController;

    private void Start() {
        behaviourTreeOwner.repeat = false;
    }

    /// <summary>
    /// Use the behaviour tree to assign a DialogTree to the DialogTreeController
    /// </summary>
    [Button][ButtonGroup]
    public void SelectDialogViaBehaviourTree() {
        behaviourTreeOwner.StartBehaviour();
    }

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
        dialogTreeController.StartDialogue(actor);
        ActiveDialogListener.Instance.OnDialogStart(dialogTreeController);
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
