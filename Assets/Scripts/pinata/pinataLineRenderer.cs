using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class pinataLineRenderer : MonoBehaviour {
    private LineRenderer lineRenderer;
    public pinataJointController pinataJointController;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        SetupLineRenderer();
    }

    private void SetupLineRenderer() {
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update() {
        // More work than necessary for this right now, but I should make connect/break unity events
        if (pinataJointController.IsJointConnected) {
            UpdateLineRenderer();
        } else {
            lineRenderer.enabled = false;
        }
    }

    public void UpdateLineRenderer() {
        lineRenderer.enabled = true;

        if (pinataJointController.connectedBodyAnchor) {
            lineRenderer.SetPosition(0, pinataJointController.connectedBodyAnchor.position);
        } else {
            lineRenderer.SetPosition(0, pinataJointController.connectedBody.transform.position);
        }

        if (pinataJointController.anchor) {
            lineRenderer.SetPosition(1, pinataJointController.anchor.position);
        } else {
            lineRenderer.SetPosition(1, pinataJointController.connectedBody.transform.position);
        }
    }


    [HideInPlayMode]
    [Button("Configure Manually")]
    public void RefreshLineRendererInInspector() {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
        UpdateLineRenderer();
    }
}
