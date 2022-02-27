using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class FinalDoor : MonoBehaviour {
    private Damageable damageable;
    public GameObject lockMesh;

    private void OnEnable() {
        damageable = GetComponent<Damageable>();
    }
    private void Start() {
        StartCoroutine(AfterInit());
    }
    private IEnumerator AfterInit() {
        yield return new WaitForEndOfFrame();
        if (SaveManager.Instance.saveData.numGamesPlayed >= 5) {
            damageable.isInvincible = false;
            lockMesh.SetActive(false);
        }
    }
}
