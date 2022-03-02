using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotPingShaderManager : MonoBehaviour {
    public Material mat;
    public Color baseColor;
    public List<Transform> points;
    public List<float> distance;
    public List<Color> colors;

    private void OnDrawGizmos() {
        if (!mat) return;

        mat.SetColor("baseColor", baseColor);

        Vector4 posSource;
        for (int i = 0; i < points.Count; i++) {
            posSource = new Vector4(points[i].position.x, points[i].position.y, points[i].position.z);
            posSource.w = distance[i];

            if (distance.Count < i) break;
            mat.SetVector($"posSource_{i}", posSource);
            if (colors.Count < i) break;
            mat.SetColor($"color_{i}", colors[i]);
        }
    }
}
