using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public HVRPlayerController playerController { get; private set; }
    public HVRSocket chestSocket;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public static Player Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }

        playerController = GetComponentInChildren<HVRPlayerController>();
        // animator = GetComponentInChildren<Animator>();
        // animHashVisionFade = Animator.StringToHash("visionFade");
        spawnPosition = playerController.transform.position;
        spawnRotation = playerController.transform.rotation;
    }

    private void Start() {
        playerController.transform.position = spawnPosition;
        playerController.transform.rotation = spawnRotation;
    }
}
