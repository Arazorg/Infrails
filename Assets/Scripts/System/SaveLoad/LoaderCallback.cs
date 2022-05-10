using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private SafeArea _safeArea;

    private bool _isFirstUpdate = true;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _safeArea.Init();
    }

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
