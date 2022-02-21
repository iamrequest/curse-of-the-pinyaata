using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not sure how better to name this class. Literally just a DialogActor with a few extra fields for per-actor customization
/// </summary>
public class DialogActorCustom : DialogueActor {
    [Tooltip("The transform that determines where the audio source will play at")]
    public Transform audioSourceTransform;

    public List<AudioClip> characterTypedSFX;
}
