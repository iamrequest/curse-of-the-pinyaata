using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;

public class DesktopCamera : MonoBehaviour {
    public Camera cam;
    public float depth;
    public Vector3 posOffset;
    public bool followPlayer;

    [Range(0f, 1f)]
    public float positionSpeed, rotationSpeed;

    private void Start() {
        cam.depth = depth;
    }

    private void Update() {
        if (followPlayer) {
            transform.position = Vector3.Slerp(transform.position, Player.Instance.playerController.Camera.position + Player.Instance.playerController.Camera.rotation * posOffset, positionSpeed);
        }

        if (HVRGlobalInputs.Instance.RightTriggerButtonState.JustActivated) {
            Screenshot();
        }
    }

    private void Screenshot() {
        DateTime dt = DateTime.Now;
        String filename = $"{dt.Year}-{dt.Month}-{dt.Day}_{dt.Hour}-{dt.Minute}-{dt.Second}-{dt.Millisecond}.png";
        Debug.Log($"Saving screenshot as ./{filename}");
        ScreenCapture.CaptureScreenshot(filename, 1);
    }
} 