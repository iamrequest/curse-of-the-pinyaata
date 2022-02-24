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

    public bool isLifetimeLimited;
    public float lifetime;

    private void OnValidate() {
        m_renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        if (isLifetimeLimited) Destroy(gameObject, lifetime);
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

            StartCoroutine(WaitForCooldown());
        }
    }

    private IEnumerator WaitForCooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
