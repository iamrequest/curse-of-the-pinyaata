using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Optional next steps: Add VFX on damage 
[RequireComponent(typeof(Damageable))]
public class ConfettiSpawner : MonoBehaviour {
    private Damageable damageable;
    public ConfettiSpawnerSettings confettiSettings;
    private Rigidbody rb;

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
    private void OnDamagedVFX(float damage, Damageable damageable, Vector3 damageSourcePosition) {
        SpawnConfetti(confettiSettings.onDamagedNumConfettiRange, damageSourcePosition);
    }

    [Button("On Death")] [ButtonGroup]
    [HideInEditorMode]
    private void SelfDestruct(Damageable damageable, Vector3 damageSourcePosition) {
        // Disable collisions to avoid colliding with confetti
        if(rb) rb.detectCollisions = false;

        // Shrink, then self-destroy after a delay
        StartCoroutine(ShrinkThenDestroy());

        // Any self-vfx would go here (eg: dust particles)

        // Note: We also spawn confetti on damage, so plan to spawn less confetti on death
        // Optional: Cache the most recent amount of confetti spawned (on damage), and figure out how much to subtract from the onDeath range
        SpawnConfetti(confettiSettings.onDeathNumConfettiRange, damageSourcePosition);
    }

    private IEnumerator ShrinkThenDestroy() {
        float t = 0f;
        Vector3 originalScale = transform.localScale;

        while (t < confettiSettings.onDestroyShrinkDuration) {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, confettiSettings.destroyAnimCurve.Evaluate(t / confettiSettings.onDestroyShrinkDuration));
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }

    [Button][ButtonGroup]
    [HideInEditorMode]
    private void SpawnConfetti(Vector2 numConfettiRange, Vector3 sourcePosition) {
        // Adding 1 to max num confetti to make the range inclusive
        int numConfettiToSpawn = Mathf.RoundToInt(UnityEngine.Random.Range(numConfettiRange.x, numConfettiRange.y + 1f));
        for (int i = 0; i < numConfettiToSpawn; i++) {
            SpawnSingleConfetti(sourcePosition);
        }
    }

    [Button][ButtonGroup]
    [HideInEditorMode]
    private void SpawnSingleConfetti(Vector3 spawnPosition) {
        GameObject confetti = Instantiate(confettiSettings.confettiPrefab, spawnPosition, UnityEngine.Random.rotation);

        if(confetti.TryGetComponent(out PingShaderCollisionInteractor shaderInteractor)) {
            // Apply random color
            shaderInteractor.m_renderer.material.color = PingShaderManager.Instance.GetRandomColor();

            // Add random velocity
            Vector3 force = new Vector3(UnityEngine.Random.Range(confettiSettings.lateralVelocityRange.x, confettiSettings.lateralVelocityRange.y),
                UnityEngine.Random.Range(confettiSettings.upwardVelocityRange.x, confettiSettings.upwardVelocityRange.y),
                UnityEngine.Random.Range(confettiSettings.lateralVelocityRange.x, confettiSettings.lateralVelocityRange.y));

            shaderInteractor.rb.AddForce(force, ForceMode.Impulse);

            shaderInteractor.spawnerRb = rb;
        }
    }
}
