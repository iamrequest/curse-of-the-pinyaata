using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class pinataJointController : MonoBehaviour {
    private Rigidbody rb;
    private SpringJoint joint;

    public Rigidbody connectedBody;
    public Transform connectedBodyAnchor, anchor;
    public SpringJointSettings jointSettings;

    public bool IsJointConnected {
        get {
            return joint != null;
        }
    }
    public Transform AnchorTransform { get {
            return anchor ? anchor : rb.transform;
        }
    }
    public Transform ConnectedAnchorTransform { get {
            return connectedBodyAnchor ? connectedBodyAnchor : connectedBody.transform;
        }
    }


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        CreateJoint();
    }


    [Button]
    [ButtonGroup("Joint Management")]
    public void CreateJoint() {
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = connectedBody;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.minDistance = Vector3.Distance(ConnectedAnchorTransform.position, AnchorTransform.position);
        jointSettings.ApplyJointSettings(joint);
    }

    [Button]
    [ButtonGroup("Joint Management")]
    public void DestroyJoint() {
        Destroy(joint);
    }

    private void FixedUpdate() {
        UpdateAnchors();
    }

    // Source: https://github.com/iamrequest/the-tall-wall-falls/blob/master/Assets/Scripts/Ropes/RopeJointManager.cs
    private void UpdateAnchors() {
        // Update the remote anchor
        if (connectedBodyAnchor != null) {
            joint.connectedAnchor = GetJointOffset(joint.connectedBody, connectedBodyAnchor);
        } else {
            joint.connectedAnchor = Vector3.zero;
        }

        // Update the local anchor
        if (anchor != null) {
            joint.anchor = GetJointOffset(rb, anchor);
        } else {
            joint.anchor = Vector3.zero;
        }
    }

    private Vector3 GetJointOffset(Rigidbody rb, Transform offsetTransform) {
        // Need to divide by the scale of the target, since the spring joint multiplies the connected anchor by the connected body's scale to find the target position.
        // In other words, I want to convert my local offset from rb to transform, into joint space
        Vector3 tmpAnchor = Quaternion.Inverse(rb.rotation) * (offsetTransform.position - rb.position);
        Vector3 offsetVector = rb.transform.lossyScale;

        tmpAnchor.x /= offsetVector.x;
        tmpAnchor.y /= offsetVector.y;
        tmpAnchor.z /= offsetVector.z;

        return tmpAnchor;
    }
}
