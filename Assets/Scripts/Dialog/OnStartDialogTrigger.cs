using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartDialogTrigger : MonoBehaviour {
    public DialogueTreeController dialogTreeController;
    public bool startAfterDelay;
    public float dialogStartDelay;

    private void OnValidate() {
        if (!dialogTreeController) {
            dialogTreeController = GetComponent<DialogueTreeController>();
        }
    }

    public void Start() {
        if (dialogTreeController) {
            if (startAfterDelay) {
                StartCoroutine(StartDialogAfterDelay());
            } else {
                dialogTreeController.StartDialogue();
            }
        }
    }

    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(dialogStartDelay);
        dialogTreeController.StartDialogue();
        ActiveDialogListener.Instance.onDialogStarted(dialogTreeController);
    }
}
