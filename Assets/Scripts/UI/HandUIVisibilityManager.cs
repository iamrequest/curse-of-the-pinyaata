using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Freya;

// Lerp alpha of UI elements based on viewing angle
public class HandUIVisibilityManager : MonoBehaviour {
    public Image backgroundImage;
    public TextMeshProUGUI textField;
    [Range(0f, 180f)]
    public float openAngle = 30f, closedAngle = 45f;

    private void Update() {
        UpdateOpacity();
    }

    private void UpdateOpacity() {
        float viewAngle = Vector3.Angle(Player.Instance.playerController.Camera.forward, transform.forward);
        float opacity = Mathfs.RemapClamped(closedAngle, openAngle, 0f, 1f, viewAngle);

        // Set background image alpha
        Color color = backgroundImage.color;
        color.a = opacity;
        backgroundImage.color = color;

        // Set text field alpha
        // TODO: This doesn't seem to work, but the text is dark enough that maybe it's not noticeable
        color = textField.faceColor;
        color.a = opacity;
        textField.faceColor = color;
    }

    private void RenderAngleToTextField() {
        float viewAngle = Vector3.Angle(Player.Instance.playerController.Camera.forward, transform.forward);
        textField.text = viewAngle.ToString("000.0");
    }

    private void RenderOpacityToTextField() {
        float viewAngle = Vector3.Angle(Player.Instance.playerController.Camera.forward, transform.forward);
        float opacity = Mathfs.RemapClamped(closedAngle, openAngle, 0, 255, viewAngle);
        textField.text = opacity.ToString("000.0");
    }
}
