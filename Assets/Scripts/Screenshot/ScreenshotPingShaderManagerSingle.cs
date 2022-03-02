using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same thing as the original renderer, but this only manages a single ping sweep - useful for animators
public class ScreenshotPingShaderManagerSingle : MonoBehaviour {
    public Material mat;
    public Color baseColor;
    public List<Transform> points;
    public float distance;
    public Color colors;

    private void OnDrawGizmos() {
        if (!mat) return;

        mat.SetColor("baseColor", baseColor);

        Vector4 posSource;
        for (int i = 0; i < points.Count; i++) {
            posSource = new Vector4(points[i].position.x, points[i].position.y, points[i].position.z);
            posSource.w = distance;

            mat.SetVector($"posSource_0", posSource);
            mat.SetColor($"color_0", colors);
        }
    }
}
