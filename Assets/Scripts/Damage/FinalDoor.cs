using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour {
    private Damageable damageable;
    public GameObject lockMesh;
    public GameObject doorBlockingCube;
    public DialogueTreeController dialogtreeController;
    public float dialogStartDelay = 1f;

    private void OnEnable() {
        damageable = GetComponentInChildren<Damageable>();
        damageable.onHealthDepleted.AddListener(OnDoorDestroyed);
    }
    private void OnDisable() {
        damageable.onHealthDepleted.RemoveListener(OnDoorDestroyed);
    }


    private void Start() {
        StartCoroutine(AfterInit());
    }
    private IEnumerator AfterInit() {
        yield return new WaitForEndOfFrame();
        if (SaveManager.Instance.saveData.numGamesPlayed >= 5) {
            damageable.isInvincible = false;
            lockMesh.SetActive(false);
        }
    }

    private void OnDoorDestroyed(Damageable arg0, Vector3 arg1) {
        GameManager.Instance.StopGame();
        doorBlockingCube.SetActive(true);
        BGMManager.Instance.FadeToStop();
        StartCoroutine(StartDialogAfterDelay());
    }
    private IEnumerator StartDialogAfterDelay() {
        yield return new WaitForSeconds(dialogStartDelay);
        dialogtreeController.StartDialogue(OnDialogComplete);
        ActiveDialogListener.Instance.OnDialogStart(dialogtreeController);
    }

    private void OnDialogComplete(bool obj) {
        SaveManager.Instance.UpdateSaveData(SaveDataKeys.isLastDoorDestroyed, true);
        GameManager.Instance.EndGame();
    }
}
