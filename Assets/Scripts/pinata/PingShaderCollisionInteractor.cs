using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spawns a ping sweep on collision
/// </summary>
public class PingShaderCollisionInteractor : MonoBehaviour {
    public LayerMask layerMask;
    public Renderer m_renderer;
    public Rigidbody rb;

    // Do not cause a ping sweep if we collide with this gameobject (eg: The gameobject that spawned this confetti)
    [HideInInspector]
    public Rigidbody spawnerRb;

    public bool applyMaterialColorToPing;
    [ReadOnly] [HideInEditorMode]
    public bool isOnCooldown;
    public float cooldown;

    [Header("Death")]
    public bool isLifetimeLimited;
    public float lifetime;
    public float onDestroyShrinkDuration;
    public AnimationCurve destroyAnimCurve;

    private Coroutine destroyAfterDelayCoroutine;

    private void OnValidate() {
        m_renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        if (isLifetimeLimited) {
            destroyAfterDelayCoroutine = StartCoroutine(DestroyAfterLifetime());
        }
    }

    private IEnumerator DestroyAfterLifetime() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private IEnumerator ShrinkThenDestroy() {
        float t = 0f;
        Vector3 originalScale = transform.localScale;

        while (t < onDestroyShrinkDuration) {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, destroyAnimCurve.Evaluate(t / onDestroyShrinkDuration));
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }


    private void OnCollisionEnter(Collision collision) {
        if (isOnCooldown) return;

        if (spawnerRb == collision.rigidbody) return;

        // Check that the colliding object matches our layermask
        if ((layerMask.value & (1 << collision.collider.gameObject.layer)) > 0) {
            if (PingShaderManager.Instance == null) return;

            if (applyMaterialColorToPing) {
                PingShaderManager.Instance.AddPing(collision.GetContact(0).point, m_renderer.material.color);
            } else {
                PingShaderManager.Instance.AddPing(collision.GetContact(0).point);
            }

            // If we created a ping, then stop the "destroy after lifetime" coroutine, and instead shrink to zero before destroying
            if(destroyAfterDelayCoroutine != null) StopCoroutine(destroyAfterDelayCoroutine);
            StartCoroutine(ShrinkThenDestroy());

            StartCoroutine(WaitForCooldown());
        }
    }

    private IEnumerator WaitForCooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
