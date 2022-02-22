using HurricaneVR.Framework.Components;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhysicsButton : MonoBehaviour {
    public bool isButtonEnabledOnStart;
    public bool isButtonEnabled { get; private set; }
    public HVRPhysicsButton hvrPhysicsButton;
    public Image buttonBGImage;
    public Color bgEnabledColor, bgDisabledColor;

    private void Start() {
        if (isButtonEnabledOnStart) {
            EnableButton();
        } else {
            DisableButton();
        }
    }

    [Button] [ButtonGroup]
    [HideInEditorMode]
    public void EnableButton() {
        if (isButtonEnabled) return;

        hvrPhysicsButton.Rigidbody.isKinematic = false;
        hvrPhysicsButton.transform.localPosition = hvrPhysicsButton.StartPosition;
        buttonBGImage.color = bgEnabledColor;

        isButtonEnabled = true;
    }

    [Button] [ButtonGroup]
    [HideInEditorMode]
    public void DisableButton() {
        hvrPhysicsButton.Rigidbody.isKinematic = true;
        hvrPhysicsButton.transform.localPosition = hvrPhysicsButton.StartPosition;
        buttonBGImage.color = bgDisabledColor;

        isButtonEnabled = false;
    }
}
