using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Confetti Spawner Settings")]
public class ConfettiSpawnerSettings : ScriptableObject {
    [Header("Confetti")]
    public GameObject confettiPrefab;
    public Vector2 onDamagedNumConfettiRange, onDeathNumConfettiRange;
    public Vector2 upwardVelocityRange, lateralVelocityRange;

    [Header("Self Destruction")]
    public float onDestroyShrinkDuration;
    public AnimationCurve destroyAnimCurve;
}
