using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fast hacky script to make one of my chars stand by default, when the default parameter value is to make them sit
[RequireComponent(typeof(Animator))]
public class OnStartAnimatorParamConfig : MonoBehaviour {
    private Animator animator;
    public string key;
    public bool value;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.SetBool(key, value);
    }
}
