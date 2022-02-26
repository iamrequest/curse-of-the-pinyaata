using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This defines how many points an object can award on damage/destroy
[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Point Holder Settings")]
public class PointHolderSettings : ScriptableObject {
    [Tooltip("The additional number of points that this object can award on death")]
    public float onDestroyPoints;
    [Tooltip("The total number of points that this object can award via damage")]
    public float totalPoints;
}
