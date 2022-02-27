using HurricaneVR.Framework.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Damageable SFX Settings")]
public class DamageableSFXSettings : ScriptableObject {
    public AudioClip damagedSFX;
    public AudioClip destroyedSFX;
    public Vector2 sfxPitchRange = new Vector2(.9f, 1.1f);

    public void PlaySFX(Damageable damageable, Vector3 worldPosition, float appliedDamage) {
        // Optional: I could override the SFX in the damageable, for custom per-damageable SFX here.
        AudioClip sfx = damageable.healthCurrent > appliedDamage ? damagedSFX : destroyedSFX;
        SFXPlayer.Instance.PlaySFXRandomPitch(sfx, worldPosition, sfxPitchRange.x, sfxPitchRange.y);
    }
}
