using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HVRGrabbable))]
public class BatSocketManager : MonoBehaviour {
    private HVRGrabbable hvrGrabbable;
    private MeshRenderer meshRenderer;
    private Coroutine returnToSocketCoroutine;

    [Range(0f, 5f)]
    public float returnToSocketDelay;

    public UnityEvent onReturnToSocket;

    private void Awake() {
        hvrGrabbable = GetComponent<HVRGrabbable>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start() {
        ReturnToSocket(true);
    }

    private void OnEnable() {
        hvrGrabbable.HandGrabbed.AddListener(OnGrabbed);
        hvrGrabbable.HandFullReleased.AddListener(OnFullRelease);
        StartCoroutine(AddListenersAfterInit());
    }
    private IEnumerator AddListenersAfterInit() {
        yield return new WaitForEndOfFrame();
        Player.Instance.chestSocket.Grabbed.AddListener(OnPlacedInSocket);
        Player.Instance.chestSocket.Released.AddListener(OnRemovedFromSocket);
    }

    private void OnDisable() {
        hvrGrabbable.HandGrabbed.RemoveListener(OnGrabbed);
        hvrGrabbable.HandFullReleased.RemoveListener(OnFullRelease);
        Player.Instance.chestSocket.Grabbed.RemoveListener(OnPlacedInSocket);
        Player.Instance.chestSocket.Released.RemoveListener(OnRemovedFromSocket);
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

    // --------------------------------------------------------------------------------
    private void ReturnToSocket(bool ignoreSocketSFX) {
        hvrGrabbable.ForceRelease();
        Player.Instance.chestSocket.TryGrab(hvrGrabbable, ignoreSocketSFX);
        onReturnToSocket.Invoke();
    }
}
