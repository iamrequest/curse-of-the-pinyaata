using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;
using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interact with simple dialog via primary buttons
/// TODO: This isn't working, need to debug
/// </summary>
public class DialogInteractorVR : DialogInteractor {
    protected override void OnEnable() {
        base.OnEnable();
    }
    protected override void OnDisable() {
        base.OnDisable();

        ActiveDialogListener.Instance.onDialogStarted -= OnDialogStarted;

        HVRControllerEvents.Instance.LeftPrimaryActivated.RemoveListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.RemoveListener(AdvanceDialogUsingRightHand);
    }

    public void Start() {
        ActiveDialogListener.Instance.onDialogStarted += OnDialogStarted;
        // HVRControllerEvents.Instance isn't initialized during OnEnable() sometimes. This fixes that
        //  Although, I should be careful when enabling/disabling this component because of this!
        HVRControllerEvents.Instance.LeftPrimaryActivated.AddListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.AddListener(AdvanceDialogUsingRightHand);
    }

    public void OnDialogStarted(DialogueTreeController dialogTreeController) {
        activeDialogController = dialogTreeController;
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
