using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

/// <summary>
/// This is a rigidbody that can collide with the pinata, which will in turn grant the player points, and create a ping sweep at the collision point
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PinataWeapon : MonoBehaviour {
    // Debug
    //public AnimationCurve loggedDamage;

    [Header("Damage")]
    public float minDamage;
    public float maxDamage;
    public float minDamagingSpeed, maxDamagingSpeed;

    private bool isDamageCooldownActive;
    public float damageCooldown;

    [Header("Ping Sweep Generation")]
    [Tooltip("A ping sweep will be created if a collision happens on this layer")]
    public LayerMask layermaskPingSweep;
    private bool isPingSweepCooldownActive;
    public float pingSweepCooldown;


    private void OnCollisionEnter(Collision collision) {
        TryPingSweep(collision);
        TryApplyDamage(collision);

    }

    // --------------------------------------------------------------------------------

    private void TryPingSweep(Collision collision) {
        if (isPingSweepCooldownActive) return;

        // Check that the colliding object matches our layermask
        if ((layermaskPingSweep.value & (1 << collision.collider.gameObject.layer)) > 0) {
            if (PingShaderManager.Instance == null) return;

            PingShaderManager.Instance.AddPing(collision.GetContact(0).point);
            StartCoroutine(WaitForPingSweepCooldown());
        }
    }

    private IEnumerator WaitForPingSweepCooldown() {
        isPingSweepCooldownActive = true;
        yield return new WaitForSeconds(pingSweepCooldown);
        isPingSweepCooldownActive = false;
    }


    // --------------------------------------------------------------------------------
    private void TryApplyDamage(Collision collision) {
        if (isDamageCooldownActive) return;

        // For now, damageables need rigidbodies. I could fix this later by checking up the hierarchy, but that's not necessary right now
        if (collision.rigidbody == null) return;

        // TODO: If I do ragdolls, this needs to be updated to get the root damageable
        if (collision.rigidbody.TryGetComponent(out Damageable damageable)) {
            float damage = CalculateDamage(collision.relativeVelocity.magnitude);
            damageable.ApplyDamage(damage);
            StartCoroutine(WaitForDamageCooldown());
        }
    }

    private IEnumerator WaitForDamageCooldown() {
        isDamageCooldownActive = true;
        yield return new WaitForSeconds(damageCooldown);
        isDamageCooldownActive = false;
    }

    private float CalculateDamage(float relativeVelocityMagnitude) {
        // Log velocity over time, to figure out damaging velocity ranges
        //loggedDamage.AddKey(Time.time, relativeVelocityMagnitude);

        if (relativeVelocityMagnitude < minDamagingSpeed) return 0f;

        float damage = Mathfs.Remap(0f, maxDamagingSpeed, minDamage, maxDamage, relativeVelocityMagnitude);
        return damage;
    }
}
