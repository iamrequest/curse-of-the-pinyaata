using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCenterOfMass : MonoBehaviour {
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody>();
        if (rb) {
            rb.centerOfMass = transform.localPosition;
        } else {
            Debug.LogWarning("No rigidbody found in the parent component. Unable to set center of mass");
        }
    }
}
