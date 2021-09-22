using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private SafeArea safeArea;

    private bool isFirstUpdate = true;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        safeArea.Init();
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
