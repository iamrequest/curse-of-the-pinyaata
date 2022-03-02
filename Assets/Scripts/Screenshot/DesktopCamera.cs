using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;

[RequireComponent(typeof(Camera))]
public class DesktopCamera : MonoBehaviour {
    private Camera cam;
    public float depth;

    [Range(0f, 1f)]
    public float positionSpeed, rotationSpeed;

    private void Start() {
        cam = GetComponent<Camera>();
        cam.depth = depth;
    }

    private void Update() {
        transform.position = Vector3.Slerp(transform.position, Player.Instance.playerController.Camera.position, positionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Player.Instance.playerController.Camera.rotation, rotationSpeed);

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