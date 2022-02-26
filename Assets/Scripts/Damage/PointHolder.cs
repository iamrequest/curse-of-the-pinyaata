using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is a gameobject that stores points, which are redeemed when the player hits it with a PinataWeapon
/// </summary>
public class PointHolder : MonoBehaviour {
    private Damageable damageable;
    public TextMeshProUGUI debugTextField;

    public float onDestroyPoints;
    public float totalPoints;
    [HideInEditorMode]
    public float remainingPoints { get; private set; }

    private void Awake() {
        damageable = GetComponent<Damageable>();
    }

    private void OnEnable() {
        if (damageable) {
            damageable.onDamaged.AddListener(AwardPointsOnDamaged);
            damageable.onHealthDepleted.AddListener(AwardPointsOnDestroyed);
        }
    }

    private void Start() {
        UpdateDebugUI();
    }

    private void AwardPointsOnDamaged(float appliedDamage, Damageable damageable, Vector3 damageSourcePosition) {
        if (damageable.healthMax == 0f) return;

        // Calculate the fraction of damage done to the damageable this round
        float awardedPoints = totalPoints * (appliedDamage / damageable.healthMax);
        awardedPoints = Mathf.Clamp(awardedPoints, 0f, remainingPoints);
        remainingPoints -= awardedPoints;
        ScoreManager.Instance.AddScore(awardedPoints);

        UpdateDebugUI();
    }

    private void UpdateDebugUI() {
        if (debugTextField) debugTextField.text = $"Pt: {remainingPoints.ToString("F1")}/{totalPoints.ToString("F1")}";
    }
    private void AwardPointsOnDestroyed(Damageable damageable, Vector3 damageSorucePosition) {
        ScoreManager.Instance.AddScore(onDestroyPoints);
    }

    // Not right now
    // public void AwardPoints(int numPoints) { }
}
