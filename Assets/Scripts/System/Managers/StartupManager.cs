using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        LoadFiles();
        AudioManager.Instance.StartAudio();
        UIManager.Instance.StartUI();
    }

    private void LoadFiles()
    {
        SettingsInfo.Instance.Load();
        PlayerProgress.Instance.Load();
        LocalizationManager.Instance.LoadLocalization(SettingsInfo.Instance.CurrentLocalization);
    }
}
