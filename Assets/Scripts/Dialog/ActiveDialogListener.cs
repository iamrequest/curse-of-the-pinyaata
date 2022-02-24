using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fake event channel. This calls UnityEvents in response to active dialog starting and stopping.
/// It's necessary to do this as a monobehaviour instead of a scriptable object, because I need to store a ref to the active dialogue tree controller
/// </summary>
public class ActiveDialogListener : MonoBehaviour {
    public static ActiveDialogListener Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple ActiveDialogListener components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    public UnityAction<DialogueTreeController> onDialogStarted;
    public UnityAction<DialogueTreeController> onDialogFinished;

    // Before we start the dialog, all active actors in the scene will be asked to try to add themselves to the DialogTreeController's actor list
    public UnityAction<DialogueTreeController> populateActorCallback;

    public DialogueTreeController activeDialogTreeController { get; private set; }


    /// <summary>
    /// Alert downstream scripts that active dialog has been started
    /// </summary>
    /// <param name="dialogueTreeController"></param>
    public void OnDialogStart(DialogueTreeController dialogueTreeController) {
        activeDialogTreeController = dialogueTreeController;
        activeDialogTreeController.behaviour.onFinish += OnDialogFinishedCallback;

        if (onDialogStarted != null) {
            onDialogStarted.Invoke(activeDialogTreeController);
        }
    }

    private void OnDialogFinishedCallback(bool graphStatus) {
        if (onDialogFinished != null) {
            onDialogFinished.Invoke(activeDialogTreeController);
        }

        activeDialogTreeController.behaviour.onFinish -= OnDialogFinishedCallback;
        activeDialogTreeController = null;
    }

    // TODO: I should figure out how to bind actors to the active dialog here, if I want the player to be able to say something
}
