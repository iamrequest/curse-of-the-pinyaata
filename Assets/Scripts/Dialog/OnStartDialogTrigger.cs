using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartDialogTrigger : MonoBehaviour {
    public ActiveDialogEventChannel activeDialogEventChannel;
    public DialogueTreeController dialogTree;
    public bool startAfterDelay;
    public float dialogStartDelay;

    private void OnValidate() {
        if (!dialogTree) {
            dialogTree = GetComponent<DialogueTreeController>();
        }
    }

    public void Start() {
        if (dialogTree) {
            if (startAfterDelay) {
                StartCoroutine(StartDialogAfterDelay());
            } else {
                dialogTree.StartDialogue();
            }
        }
    }

    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(dialogStartDelay);
        activeDialogEventChannel.StartDialog(dialogTree);
    }
}
