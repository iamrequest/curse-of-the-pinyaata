using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {
    public TextMeshProUGUI debugTextField;
    public DamageableSFXSettings sfxSettings;

    public bool isInvincible;
    public bool isInvincibleBeforeGame;

    [HideInEditorMode]
    public float healthCurrent;
    public float healthMax;

    [FoldoutGroup("Health Events")]
    public OnHealthChangedEvent onDamaged;
    [FoldoutGroup("Health Events")]
    public OnDeathEvent onHealthDepleted;


    private void Start() {
        healthCurrent = healthMax;
        UpdateDebugUI();
    }

    public void ApplyDamage(float incomingDamage) {
        ApplyDamage(incomingDamage, transform.position);
    }

    public void ApplyDamage(float incomingDamage, Vector3 damagePosition) {
        if (healthCurrent <= 0f) return;
        if (isInvincible) return;
        if (isInvincibleBeforeGame) {
            if (GameManager.Instance.gameState == GameState.NOT_STARTED) return;
        }

        // Play damage SFX
        sfxSettings.PlaySFX(this, damagePosition, incomingDamage);

        // Calculate incoming damage
        float originalHealth = healthCurrent;
        healthCurrent = Mathf.Clamp(healthCurrent - incomingDamage, 0f, healthMax);
        float appliedDamage = originalHealth - healthCurrent;

        // Invoke event
        onDamaged.Invoke(appliedDamage, this, damagePosition);

        if (healthCurrent <= 0) {
            onHealthDepleted.Invoke(this, damagePosition);
        }

        UpdateDebugUI();
    }

    private void UpdateDebugUI() {
        if (debugTextField) debugTextField.text = $"HP: {healthCurrent.ToString("F1")}/{healthMax.ToString("F1")}";
    }


    [Button("Apply 1 Damage")]
    [ButtonGroup][HideInEditorMode]
    private void ApplyOneDamge() {
        ApplyDamage(1f);
    }

    [Button]
    [ButtonGroup][HideInEditorMode]
    public void Kill() {
        ApplyDamage(healthMax + 1);
    }
}
