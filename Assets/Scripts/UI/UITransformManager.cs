using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aligns world UI to optionally follow a position transform, and/or look at the player's camera
public class UITransformManager : MonoBehaviour {
    public Transform targetPositionTransform;

    public bool alignPositionToTarget;
    public bool lookatPlayer;

    private void Update() {
        if (alignPositionToTarget) AlignPosition();
        if (lookatPlayer) LookAtPlayer();
    }

    public void AlignPosition() {
        if(targetPositionTransform) transform.position = targetPositionTransform.position;
    }

    public void LookAtPlayer() {
        transform.forward = Vector3.ProjectOnPlane(Player.Instance.playerController.Camera.position - transform.position, Vector3.up).normalized;
    }
}
