using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDialogPicker : DialogPicker {
    public float dialogStartDelay;


    private void OnEnable() {
        StartCoroutine(AfterInit());
    }


    protected override void Start() {
        base.Start();
        // Pick some dialog to say, and start it as soon as it's picked
        SelectDialogViaBehaviourTree(StartDialogAfterPicked);
    }
    private IEnumerator AfterInit() {
        yield return new WaitForEndOfFrame();
        BGMManager.Instance.PlaySong(0);
    }

    private void StartDialogAfterPicked(bool obj) {
        StartCoroutine(StartDialogAfterDelay());
    }

    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(dialogStartDelay);

        if (dialogTreeController.behaviour) {
            StartSelectedDialog();
        } else {
            // No dialog took place, but we should call the event anyways just in case
            Debug.Log("No dialog selected");
            onDialogComplete.Invoke();
        }
    }
}
