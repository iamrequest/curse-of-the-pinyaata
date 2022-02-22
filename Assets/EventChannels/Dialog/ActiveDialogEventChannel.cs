using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invoke RaiseEvent() whenever the player actively starts a dialog
///     This would be different from passive dialog (eg: an NPC remarking something to themselves, or NPCs talking with each-other)
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Active Dialog Event Channel")]
public class ActiveDialogEventChannel : ScriptableObject {
    public UnityAction<DialogueTreeController> onDialogStarted;

    public void StartDialog(DialogueTreeController dialog) {
        if (onDialogStarted != null) {
            onDialogStarted.Invoke(dialog);
            dialog.StartDialogue();
        }
    }
}
