using NodeCanvas.DialogueTrees;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not sure how better to name this class. Literally just a DialogActor with a few extra fields for per-actor customization
/// </summary>
public class DialogActorCustom : DialogueActor {
    public Animator animator;
    public Vector2 animatorSpeedRange;

    [Tooltip("The transform that determines where the audio source will play at")]
    public Transform audioSourceTransform;
    public List<AudioClip> characterTypedSFX;
    [Tooltip("Only play the char typed SFX every x characters. Useful for when the char typed SFX gets annoying when played frequently")]
    [Range(1, 5)]
    public int charTypedSFXSkip;

    private void OnEnable() {
        StartCoroutine(AddListenersAfterInit());
    }
    private IEnumerator AddListenersAfterInit() {
        yield return new WaitForEndOfFrame();
        ActiveDialogListener.Instance.populateActorCallback += AddActorReference;
    }
    private void OnDisable() {
        ActiveDialogListener.Instance.populateActorCallback -= AddActorReference;
    }

    private void Start() {
        if(animator) animator.speed = Random.Range(animatorSpeedRange.x, animatorSpeedRange.y);
    }

    private void AddActorReference(DialogueTreeController dialogueTreeController) {
        if (dialogueTreeController == null) return;
        if (dialogueTreeController.behaviour == null) return;

        // Check if the dialog references this actor. If it does, add the IDialogActor reference
        // For future works: It would be better to instead compare some unique ID, rather than the display name. 
        //  This way, I could (probably) dynamically change the actor's name as needed. 
        //  Eg: If you don't know the person's name yet, render their name as "???"
        if (dialogueTreeController.behaviour.definedActorParameterNames.Contains(name)) {
            dialogueTreeController.behaviour.SetActorReference(name, this);
        }
    }
}
