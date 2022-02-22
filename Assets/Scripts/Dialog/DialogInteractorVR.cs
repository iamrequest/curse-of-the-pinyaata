using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;
using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interact with simple dialog via primary buttons
/// TODO: This isn't working, need to debug
/// </summary>
public class DialogInteractorVR : DialogInteractor {
    public ActiveDialogEventChannel activeDialogEventChannel;

    protected override void OnEnable() {
        base.OnEnable();

        activeDialogEventChannel.onDialogStarted += OnDialogStarted;
    }
    protected override void OnDisable() {
        base.OnDisable();

        activeDialogEventChannel.onDialogStarted -= OnDialogStarted;

        HVRControllerEvents.Instance.LeftPrimaryActivated.RemoveListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.RemoveListener(AdvanceDialogUsingRightHand);
    }

    public void Start() {
        // HVRControllerEvents.Instance isn't initialized during OnEnable() sometimes. This fixes that
        //  Although, I should be careful when enabling/disabling this component because of this!
        HVRControllerEvents.Instance.LeftPrimaryActivated.AddListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.AddListener(AdvanceDialogUsingRightHand);
    }

    public void OnDialogStarted(DialogueTreeController dialogTree) {
        activeDialogController = dialogTree;
    }


    /// <summary>
    /// Advance dialog if the target hand is empty
    /// </summary>
    public void AdvanceDialogUsingLeftHand() { AdvanceDialogUsingHand(); }
    public void AdvanceDialogUsingRightHand() { AdvanceDialogUsingHand(); }

    public void AdvanceDialogUsingHand() {
        AdvanceDialog();
    }
}
