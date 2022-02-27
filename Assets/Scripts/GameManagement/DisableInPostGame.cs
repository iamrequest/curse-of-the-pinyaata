using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInPostGame : MonoBehaviour {
    private void Start() {
        // Check if the save manager is ready - if so, try to set the object as inactive
        // If it works, this prevents the object from showing up on the first frame
        if (SaveManager.Instance) {
            TrySetInactive();
        } else {
            StartCoroutine(AfterInit());
        }
    }
    private IEnumerator AfterInit() {
        yield return new WaitForEndOfFrame();
        TrySetInactive();
    }

    private void TrySetInactive() {
        if (SaveManager.Instance.saveData.finalDoorBroken) {
            gameObject.SetActive(false);
        }
    }
}
