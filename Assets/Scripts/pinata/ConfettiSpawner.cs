using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Optional next steps: Add VFX on damage 
[RequireComponent(typeof(Damageable))]
public class ConfettiSpawner : MonoBehaviour {
    private Damageable damageable;
    [Header("Confetti")]
    public GameObject confettiPrefab;
    public Vector2 onDamagedNumConfettiRange, onDeathNumConfettiRange;
    public Vector2 upwardVelocityRange, lateralVelocityRange;

    [Header("Self Destruction")]
    public Rigidbody rb;
    public float destroyDelay;

    private void OnEnable() {
        damageable = GetComponent<Damageable>();

        damageable.onDamaged.AddListener(OnDamagedVFX);
        damageable.onHealthDepleted.AddListener(SelfDestruct);
    }
    private void OnDisable() {
        damageable.onDamaged.RemoveListener(OnDamagedVFX);
        damageable.onHealthDepleted.RemoveListener(SelfDestruct);
    }

    [Button("On Damaged")] [ButtonGroup]
    [HideInEditorMode]
    private void OnDamagedVFX(float damage, Damageable damageable) {
        SpawnConfetti(onDamagedNumConfettiRange);
    }

    [Button("On Death")] [ButtonGroup]
    [HideInEditorMode]
    private void SelfDestruct(Damageable damageable) {
        // Disable collisions to avoid colliding with confetti
        rb.detectCollisions = false;

        // Self destruct after a delay
        Destroy(gameObject, destroyDelay);

        // Any self-vfx would go here (eg: dust particles)

        // Note: We also spawn confetti on damage, so plan to spawn less confetti on death
        // Optional: Cache the most recent amount of confetti spawned (on damage), and figure out how much to subtract from the onDeath range
        SpawnConfetti(onDeathNumConfettiRange);
    }

    [Button][ButtonGroup]
    [HideInEditorMode]
    private void SpawnConfetti(Vector2 numConfettiRange) {
        // Adding 1 to max num confetti to make the range inclusive
        int numConfettiToSpawn = Mathf.RoundToInt(UnityEngine.Random.Range(numConfettiRange.x, numConfettiRange.y + 1f));
        for (int i = 0; i < numConfettiToSpawn; i++) {
            SpawnSingleConfetti();
        }
    }

    [Button][ButtonGroup]
    [HideInEditorMode]
    private void SpawnSingleConfetti() {
        GameObject confetti = Instantiate(confettiPrefab, transform.position, UnityEngine.Random.rotation);

        if(confetti.TryGetComponent(out PingShaderCollisionInteractor shaderInteractor)) {
            // Apply random color
            shaderInteractor.m_renderer.material.color = PingShaderManager.Instance.GetRandomColor();

            // Add random velocity
            Vector3 force = new Vector3(UnityEngine.Random.Range(lateralVelocityRange.x, lateralVelocityRange.y),
                UnityEngine.Random.Range(upwardVelocityRange.x, upwardVelocityRange.y),
                UnityEngine.Random.Range(lateralVelocityRange.x, lateralVelocityRange.y));

            shaderInteractor.rb.AddForce(force, ForceMode.Impulse);

            shaderInteractor.spawnerRb = rb;
        }
    }
}
