﻿using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogInteractor : MonoBehaviour {
    public SceneManagerEventChannel sceneManagerEventChannel;
    private DialogueTreeController dialogueTreeController;
    private DialogActorCustom dialogActor;
    [Range(0.1f, 1f)]
    public float dialogStartDelay = 1f;
    public bool batFirstGrab, pinataDestroyed;

    private void Awake() {
        dialogueTreeController = GetComponent<DialogueTreeController>();
        dialogActor = GetComponent<DialogActorCustom>();
    }
    private void Start() {
        // For some reason, I need to add a bit of delay before I can start dialog. Maybe to let something init?
        StartCoroutine(StartDialogAfterDelay());
    }

    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(dialogStartDelay);
        dialogueTreeController.StartDialogue(dialogActor);
        ActiveDialogListener.Instance.OnDialogStart(dialogueTreeController);
    }

    public void OnBatGrabbed() {
        batFirstGrab = true;
        dialogueTreeController.StartDialogue(dialogActor);
        ActiveDialogListener.Instance.OnDialogStart(dialogueTreeController);
    }
    public void OnPinataDestroyed() {
        pinataDestroyed = true;
        dialogueTreeController.StartDialogue(dialogActor);
        ActiveDialogListener.Instance.OnDialogStart(dialogueTreeController);
    }

    public void ReturnToStartMenu() {
        sceneManagerEventChannel.LoadTitleScene();
    }
}