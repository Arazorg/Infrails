using System.Collections;
using UnityEngine;

public class PauseUI : BaseUI, IUIPanel
{
    [SerializeField] private ExitUI _exitUI;
    [SerializeField] private SettingsUI _settingsUI;

    private bool _isSettingsPanelOpen;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public bool IsSettingsPanelOpen
    {
        set { _isSettingsPanelOpen = value; }
    }

    public void OnPush()
    {
        Open();
    }

    public void OnPop()
    {
        Close();
    }

    public void OnShow()
    {
        Show();
    }

    public void OnHide()
    {
        _isSettingsPanelOpen = false;
        Hide();
    }

    public void Open()
    {
        Time.timeScale = 0f;
        _isPopAvailable = true;
        StartCoroutine(EnableBackButton());
        Show();
    }

    public void BackToGame()
    {
        UIManager.Instance.UIStackPop();
    }

    public void ShowHideSettingsPanel()
    {
        _isSettingsPanelOpen = !_isSettingsPanelOpen;
        if (_isSettingsPanelOpen)
            _settingsUI.Show();
        else
            _settingsUI.Hide();
    }

    public void ShowExitUI()
    {
        UIManager.Instance.UIStackPush(_exitUI);
    }

    private void Close()
    {
        Time.timeScale = 1f;
        _isBackButtonEnabled = false;
        _isSettingsPanelOpen = false;
        Hide();
    }

    private IEnumerator EnableBackButton()
    {
        float enableBackButtonDelay = 0.25f;
        yield return new WaitForSecondsRealtime(enableBackButtonDelay);
        _isBackButtonEnabled = true;
    }
}
