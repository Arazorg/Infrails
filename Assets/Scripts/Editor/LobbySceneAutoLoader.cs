using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class LobbySceneAutoLoader
{
    private const string FirstScenePath = "Assets/Scenes/FirstSceneForLoad.unity";
    private const string PrefKeyPrevScene = "PREVIOUS SCENE";
    static LobbySceneAutoLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChange;
    }

    private static void OnPlayModeStateChange(PlayModeStateChange state)
    {
        if(!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                return;
            }

            var currentScenePath = SceneManager.GetActiveScene().path;
            EditorPrefs.SetString(PrefKeyPrevScene, currentScenePath);

            if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene(FirstScenePath);
                }
                catch
                {
                    Debug.Log(string.Format($"Cannot load scene: {FirstScenePath}"));
                    EditorApplication.isPlaying = false;
                }
            }
            else
            {
                EditorApplication.isPlaying = false;
            }
        }

        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            var path = EditorPrefs.GetString(PrefKeyPrevScene);
            try
            {                
                EditorSceneManager.OpenScene(path);
            }
            catch
            {
                Debug.Log(string.Format($"Cannot load scene: {path}"));
            }
        }
    }
}
