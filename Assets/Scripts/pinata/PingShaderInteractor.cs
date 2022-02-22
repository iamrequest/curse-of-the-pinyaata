using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingShaderInteractor : MonoBehaviour {
    [ReadOnly] [HideInEditorMode]
    public bool isOnCooldown;
    public float cooldown;

    public LayerMask layerMask;

    private void OnCollisionEnter(Collision collision) {
        if (isOnCooldown) return;

        // Check that the colliding object matches our layermask
        if ((layerMask.value & (1 << collision.collider.gameObject.layer)) > 0) {
            if (PingShaderManager.Instance == null) return;

            PingShaderManager.Instance.AddPing(collision.GetContact(0).point);
            StartCoroutine(WaitForCooldown());
        }
    }

    private IEnumerator WaitForCooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
