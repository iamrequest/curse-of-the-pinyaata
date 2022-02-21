using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;
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

        HVRControllerEvents.Instance.LeftPrimaryActivated.AddListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.AddListener(AdvanceDialogUsingRightHand);
    }
    protected override void OnDisable() {
        base.OnDisable();

        HVRControllerEvents.Instance.LeftPrimaryActivated.RemoveListener(AdvanceDialogUsingLeftHand);
        HVRControllerEvents.Instance.RightPrimaryActivated.RemoveListener(AdvanceDialogUsingRightHand);
    }

    /// <summary>
    /// Advance dialog if the target hand is empty
    /// </summary>
    public void AdvanceDialogUsingLeftHand() { AdvanceDialogUsingHand(); }
    public void AdvanceDialogUsingRightHand() { AdvanceDialogUsingHand(); }

    public void AdvanceDialogUsingHand() {
        Debug.Log("VR INput seen");
        AdvanceDialog();
    }
}
