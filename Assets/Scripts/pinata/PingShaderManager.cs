using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;
using System;

[System.Serializable]
public class PingSweepData {
    public bool isActive;
    [HideInInspector]
    public float activeDuration = 0f;
    public Vector4 positionSource;
}

// This class could be optimized by using coroutines, and maybe by keeping better track of which ping sweep slots are available
public class PingShaderManager : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;

    // Optimization: If a color's alpha channel is 0, do not compute that ping sweep slot's color.
    private Color unusedColor = new Color(0, 0, 0, 0);
    public bool DEBUG_UPDATE_PINGS = true;

    [Header("Material Settings")]

    // The number of ping sweeps that can be active at one time
    // This is configured at the shader level, since I have to add a sub-graph for each ping sweep
    public Material pingSweepMaterial;
    public int maxNumPasses;
    public float pingSpeed;
    public float pingDuration;

    // The distance from the origin for each active ping sweep
    public List<PingSweepData> pingSweeps;
    public List<Color> colors;

    [Header("Transition")]
    public float blindfoldTransitionDuration;

    public static PingShaderManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple PingShaderManager components detected. This is probably a bug.");
            Destroy(this);
        }

        // Initialize the array of ping sweep data
        pingSweeps = new List<PingSweepData>(maxNumPasses);
        for (int i = 0; i < maxNumPasses; i++) {
            pingSweeps.Add(new PingSweepData());

            // Reset ping colors in materials
            pingSweepMaterial.SetColor($"color_{i}", unusedColor);
        }
    }
    private void Start() {
        RemoveBlindfoldImmediate();
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChanged -= OnGameStateChanged;
    }

    private void Update() {
        if(DEBUG_UPDATE_PINGS) UpdatePings();
    }

    private void OnGameStateChanged(GameState newGameState) {
        switch (newGameState) {
            case GameState.PREGAME:
                ApplyBlindfold();
                break;
            case GameState.FINISHED:
                RemoveBlindfold();
                break;
        }
    }


    [Button] [HideInEditorMode]
    public void AddPing(Vector3 worldPosition) {
        AddPing(worldPosition, GetRandomColor());
    }

    public void AddPing(Vector3 worldPosition, Color color) {
        // TODO: This should be optimized, to prevent iterating over the whole array each time we want to check if there's an available slot
        int index = GetEmptySlotForPingSweep();
        if (index == -1) {
            return;
        }

        pingSweeps[index].positionSource = worldPosition;
        pingSweeps[index].isActive = true;
        pingSweeps[index].activeDuration = 0f;

        pingSweepMaterial.SetVector($"posSource_{index}", worldPosition);
        pingSweepMaterial.SetColor($"color_{index}", color);
    }

    public Color GetRandomColor() {
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }

    /// <summary>
    /// Iterate over the list of ping sweeps to find an inactive ping sweep index
    /// </summary>
    private int GetEmptySlotForPingSweep() {
        // TODO: Safety check against the case where pingSweeps doesn't have a capacity of maxNumPasses. That shouldn't happen
        for (int i = 0; i < maxNumPasses; i++) {
            if (!pingSweeps[i].isActive) return i;
        }

        return -1;
    }

    private void UpdatePings() {
        for(int i = 0; i < maxNumPasses; i++) {
            if (i > pingSweeps.Count - 1) {
                Debug.LogError("Attempted to update ping sweeps, but we're looking at an index that doesn't exist");
                return;
            }

            if (pingSweeps[i].isActive) {
                // Update distance calculation
                pingSweeps[i].positionSource.w += Time.deltaTime * pingSpeed;
                pingSweepMaterial.SetVector($"posSource_{i}", pingSweeps[i].positionSource);

                // Delete old ping sweeps
                pingSweeps[i].activeDuration += Time.deltaTime;
                if (pingSweeps[i].activeDuration >= pingDuration) {
                    pingSweeps[i].isActive = false;

                    // See optimization note above
                    pingSweepMaterial.SetColor($"color_{i}", unusedColor);
                }
            }
        }
    }

    // --------------------------------------------------------------------------------
    private IEnumerator DoBlindfoldTransition(bool applyBlindfold) {
        float elapsedDuration = 0f;
        float a;
        Color baseColor = pingSweepMaterial.GetColor("baseColor");

        float from = applyBlindfold ? 0f : 1f;
        float to = applyBlindfold ? 1f : 0f;

        while (elapsedDuration < blindfoldTransitionDuration) {
            elapsedDuration += Time.deltaTime;
            a = Mathfs.RemapClamped(0f, blindfoldTransitionDuration, from, to, elapsedDuration);
            baseColor.a = a;
            pingSweepMaterial.SetColor("baseColor", baseColor);
            yield return null;
        }
    }

    [Button][ButtonGroup("Blindfold")]
    public void ApplyBlindfold() {
        StartCoroutine(DoBlindfoldTransition(true));
    }
    [Button][ButtonGroup("Blindfold")]
    public void RemoveBlindfold() {
        StartCoroutine(DoBlindfoldTransition(false));
    }

    [Button][ButtonGroup("Blindfold")]
    public void ApplyBlindfoldImmediate() {
        Color baseColor = pingSweepMaterial.GetColor("baseColor");
        baseColor.a = 1f;
        pingSweepMaterial.SetColor("baseColor", baseColor);
    }
    [Button][ButtonGroup("Blindfold")]
    public void RemoveBlindfoldImmediate() {
        Color baseColor = pingSweepMaterial.GetColor("baseColor");
        baseColor.a = 0f;
        pingSweepMaterial.SetColor("baseColor", baseColor);
    }
}
