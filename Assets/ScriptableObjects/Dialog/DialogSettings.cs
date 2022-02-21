using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Dialog Settings")]
public class DialogSettings : ScriptableObject {
	public List<AudioClip> defaultCharacterTypedSFX;

	public AudioClip GetRandomCharTypedSFX() { 
		int randomIndex = UnityEngine.Random.Range(0, defaultCharacterTypedSFX.Count);
		return defaultCharacterTypedSFX[randomIndex];
	}
}
