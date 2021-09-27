using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public enum Scene
    {
        Lobby,
        Loading,
        Game
    }

    public static void Load(Scene scene, bool isOpenLoadingScene = true)
    {
        if (isOpenLoadingScene)
        {
            SceneManager.LoadScene(Scene.Loading.ToString());
        }
        
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        float loadDelay = 1.5f;
        yield return new WaitForSeconds(loadDelay);
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!loadingAsyncOperation.isDone)
            yield return null;
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
            return loadingAsyncOperation.progress;
        else
            return 0f;
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    private class LoadingMonoBehaviour : MonoBehaviour { }
}
