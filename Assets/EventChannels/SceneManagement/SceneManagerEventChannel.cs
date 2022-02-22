using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Usage: Invoke RaiseEvent() whenever we need to switch scenes
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Scene Manager Event Channel")]
public class SceneManagerEventChannel : ScriptableObject {
    // These events are responsible for invoking an action
    public UnityAction<string> beforeSceneLoad;

    public string titleSceneName, gameSceneName;

    public void LoadTitleScene() { LoadScene(titleSceneName); }
    public void LoadGameScene() { LoadScene(gameSceneName);  }

    private void LoadScene(string sceneName) {
        // Make sure that this scene exists in build settings
        if (SceneManager.GetSceneByName(sceneName) == null) {
            Debug.LogError("Attempted to load a scene, but no scene with that name exists in build settings");
            return;
        }

        // Alert downstream scripts, if necessary
        if (beforeSceneLoad != null) {
            beforeSceneLoad.Invoke(sceneName);
        }

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
