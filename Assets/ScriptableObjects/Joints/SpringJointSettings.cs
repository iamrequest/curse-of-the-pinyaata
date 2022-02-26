using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Spring Joint Settings")]
public class SpringJointSettings : ScriptableObject {
    public float maxDistance;
    public float spring, damper;


    public void ApplyJointSettings(SpringJoint springJoint) {
        springJoint.autoConfigureConnectedAnchor = false;
        //springJoint.minDistance = minDistance;
        springJoint.maxDistance = maxDistance;
        springJoint.spring = spring;
        springJoint.damper = damper;
    }
}
