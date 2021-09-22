using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        LocalizationManager.LoadLocalizedText(SettingsInfo.Instance.CurrentLocalization);
    }
}
