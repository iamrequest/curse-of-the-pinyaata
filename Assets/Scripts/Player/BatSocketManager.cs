using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatSocketManager : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;

    public HVRGrabbable hvrGrabbable;
    private MeshRenderer meshRenderer;
    private Coroutine returnToSocketCoroutine;
    public HVRSocket chestSocket;

    [Range(0f, 5f)]
    public float returnToSocketDelay;

    public UnityEvent onReturnToSocket;

    private void Awake() {
        meshRenderer = hvrGrabbable.GetComponentInChildren<MeshRenderer>();
    }

    private void Start() {
        ReturnToSocket(true);
        chestSocket.gameObject.SetActive(false);
    }

    private void OnEnable() {
        hvrGrabbable.HandGrabbed.AddListener(OnGrabbed);
        hvrGrabbable.HandFullReleased.AddListener(OnFullRelease);
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
        StartCoroutine(AddListenersAfterInit());
    }

    private IEnumerator AddListenersAfterInit() {
        yield return new WaitForEndOfFrame();
        chestSocket.Grabbed.AddListener(OnPlacedInSocket);
        chestSocket.Released.AddListener(OnRemovedFromSocket);
    }

    private void OnDisable() {
        hvrGrabbable.HandGrabbed.RemoveListener(OnGrabbed);
        hvrGrabbable.HandFullReleased.RemoveListener(OnFullRelease);
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
        chestSocket.Grabbed.RemoveListener(OnPlacedInSocket);
        chestSocket.Released.RemoveListener(OnRemovedFromSocket);
    }


    private void OnGrabbed(HVRHandGrabber arg0, HVRGrabbable arg1) {
        if (returnToSocketCoroutine != null) {
            StopCoroutine(returnToSocketCoroutine);
            returnToSocketCoroutine = null;
        }
    }

    private void OnFullRelease(HVRHandGrabber arg0, HVRGrabbable arg1) {
        returnToSocketCoroutine = StartCoroutine(ReturnToSocketAfterDelay());
    }

    private IEnumerator ReturnToSocketAfterDelay() {
        yield return new WaitForSeconds(returnToSocketDelay);
        ReturnToSocket(false);
    }

    private void OnRemovedFromSocket(HVRGrabberBase arg0, HVRGrabbable arg1) {
        meshRenderer.enabled = true;
    }

    private void OnPlacedInSocket(HVRGrabberBase arg0, HVRGrabbable arg1) {
        meshRenderer.enabled = false;
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.ACTIVE:
                chestSocket.gameObject.SetActive(true);
                ReturnToSocket(true);
                break;
            case GameState.FINISHED:
                ReturnToSocket(false);
                chestSocket.gameObject.SetActive(false);
                break;
        }
    }

    // --------------------------------------------------------------------------------
    private void ReturnToSocket(bool ignoreSocketSFX) {
        hvrGrabbable.ForceRelease();
        chestSocket.TryGrab(hvrGrabbable, ignoreSocketSFX);
        meshRenderer.enabled = false;
        onReturnToSocket.Invoke();
    }
}
