using HurricaneVR.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HVRGlobalSpawner : MonoBehaviour {
    public HVRManager hvrManager;
    private void Awake() {
        hvrManager = GetComponent<HVRManager>();
        if (HVRManager.Instance == null) return;
        if (hvrManager != HVRManager.Instance) Destroy(gameObject);
    }
}
