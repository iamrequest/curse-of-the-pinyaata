using HurricaneVR.Framework.Core.Utils;
using NodeCanvas.DialogueTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NodeCanvas.DialogueTrees.UI.Examples.DialogueUGUI;

public class DialogRenderer : MonoBehaviour {
	public DialogSettings dialogSettings;

	// Optional: Only render for this actor. Greyscale the dialog BG image when someone else is talking
	public DialogueActor dialogActor;
	public RectTransform uiTransform;

	public Image actorNameImage, sentenceImage;
	public TextMeshProUGUI actorNameTextField, sentenceTextField;
	public SubtitleDelays subtitleDelays = new SubtitleDelays();

	private bool sentenceSkipRequested;
	private Coroutine typeSentenceCoroutine;

	void OnEnable() {
		DialogueTree.OnDialogueStarted       += OnDialogueStarted;
		//DialogueTree.OnDialoguePaused        += OnDialoguePaused;
		DialogueTree.OnDialogueFinished      += OnDialogueFinished;
		DialogueTree.OnSubtitlesRequest      += OnSubtitlesRequest;
		DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
	}

	void OnDisable(){
		DialogueTree.OnDialogueStarted       -= OnDialogueStarted;
		//DialogueTree.OnDialoguePaused        -= OnDialoguePaused;
		DialogueTree.OnDialogueFinished      -= OnDialogueFinished;
		DialogueTree.OnSubtitlesRequest      -= OnSubtitlesRequest;
		DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
	}

    void Start() {
		uiTransform.gameObject.SetActive(false);
	}

    void Update() { }

	public void SkipSentence() {
		sentenceSkipRequested = true;
	}


	private void OnDialogueStarted(DialogueTree dialogueTree) { }
	private void OnDialogueFinished(DialogueTree dialogueTree) { 
		uiTransform.gameObject.SetActive(false);
	}

	private void OnSubtitlesRequest(SubtitlesRequestInfo info) {
		if (info.actor != dialogActor) {
			uiTransform.gameObject.SetActive(false);
			return;
		}

		// Show the dialog box
		uiTransform.gameObject.SetActive(true);

		// Prepare the text for displaying
		sentenceTextField.maxVisibleCharacters = 0;
		sentenceTextField.text = info.statement.text;
		actorNameTextField.text = info.actor.name;

		sentenceSkipRequested = false;

		// Set the tint of the dialog box images
		actorNameImage.color = info.actor.dialogueColor;
		sentenceImage.color = info.actor.dialogueColor;

		// Start a new coroutine to type out each character
		if (typeSentenceCoroutine != null) StopCoroutine(typeSentenceCoroutine);
		typeSentenceCoroutine = StartCoroutine(TypeSentence(info));
	}

	private IEnumerator TypeSentence(SubtitlesRequestInfo info) {
		// Initial delay
		yield return new WaitForSeconds(subtitleDelays.sentenceDelay);

		for (int c = 0; c < info.statement.text.Length; c++) {
			// Skip this sentence
			if (sentenceSkipRequested) {
				sentenceTextField.maxVisibleCharacters = info.statement.text.Length;
				break;
			}

			// Per-character delay
			yield return new WaitForSeconds(GetCharTypedDelay(info.statement.text[0]));

			PlayCharacterTypedSFX(info);
			sentenceTextField.maxVisibleCharacters++;
		}

		// Final delay
		yield return new WaitForSeconds(subtitleDelays.finalDelay);
	}

	private float GetCharTypedDelay(char c) {
		// Optional: Add a custom override delay on a per-sentence basis
		if (c == ',') return subtitleDelays.commaDelay;
		if (c == '.' || c == '!' || c == '?') return subtitleDelays.finalDelay;
		return subtitleDelays.characterDelay;
	}

	private void PlayCharacterTypedSFX(SubtitlesRequestInfo info) {
		// Play character SFX
		DialogActorCustom actor = info.actor as DialogActorCustom;
		Vector3 audioSourcePosition = actor ? actor.audioSourceTransform.position : transform.position;

		SFXPlayer.Instance.PlaySFXRandomPitch(GetCharacterTypedSFX(actor),
			audioSourcePosition,
			0.9f,
			1.1f);
	}

	/// <summary>
	/// If the supplied actor is not null, and has at least 1 SFX to play, pick a random one. Otherwise, use the default.
	/// </summary>
	/// <param name="actor"></param>
	/// <returns></returns>
	private AudioClip GetCharacterTypedSFX(DialogActorCustom actor) {
		int randomIndex;

		if (actor != null) {
			if (actor.characterTypedSFX.Count > 0) {
				randomIndex = UnityEngine.Random.Range(0, actor.characterTypedSFX.Count);
				return actor.characterTypedSFX[randomIndex];
			}
		}

		return dialogSettings.GetRandomCharTypedSFX();
	}

	// TODO: Need to render multiple choice selection text plus option buttons
	// This may be better done on the the player interactor, rather than a general dialog renderer
	private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info) { }
}
