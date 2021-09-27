using System.Collections;
using UnityEngine;

public class LobbyUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private ExitUI _exitUI;
    [SerializeField] private ShopMainPanelUI _shopMainPanelUI;
    [SerializeField] private ThanksUI _thanksUI;
    [SerializeField] private SettingsUI _settingsUI;

    [Header("Animations UI Scripts")]
    [SerializeField] private AnimationsUI _lobbyButtonsPanel;
    [SerializeField] private AnimationsUI _localizationPanel;

    private bool _isSettingsPanelOpen;
    private bool _isLocalizationPanelOpen;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public bool IsSettingsPanelOpen { get => _isSettingsPanelOpen; set => _isSettingsPanelOpen = value; }

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
        StopAllCoroutines();
        StartCoroutine(EnableBackButton());
        Show();
        LobbyEnvironmentManager.Instance.SetLobbyObjectsСlickability(true);
    }

    public void OnHide()
    {
        Close();
    }

    public void Open()
    {
        CurrentGameInfo.Instance.CreateNewGame();
        LobbyEnvironmentManager.Instance.StartSpawn();
        _isSettingsPanelOpen = false;
        _isPopAvailable = false;

        if (PlayerProgress.Instance.IsLobbyTutorialCompleted)
            OnShow();
    }

    public void Close()
    {
        _isBackButtonEnabled = false;
        Hide();
    }

    public void OpenExitUI()
    {
        UIManager.Instance.UIStackPush(_exitUI);
    }

    public void OpenShopUI()
    {
        UIManager.Instance.UIStackPush(_shopMainPanelUI);
    }

    public void OpenThanksUI()
    {
        UIManager.Instance.UIStackPush(_thanksUI);
    }

    public void OpenCloseSettings()
    {
        _isSettingsPanelOpen = !_isSettingsPanelOpen;
        if (_isSettingsPanelOpen)
        {
            _lobbyButtonsPanel.Hide();
            _settingsUI.Show();
        }
        else
        {
            _lobbyButtonsPanel.Show();
            _settingsUI.Hide();
        }
    }

    public void OpenCloseLocalizationPanel()
    {
        _isLocalizationPanelOpen = !_isLocalizationPanelOpen;
        if (_isLocalizationPanelOpen)
        {
            _localizationPanel.Show();
            _settingsUI.Hide();
        }
        else
        {
            _isSettingsPanelOpen = true;
            _localizationPanel.Hide();
            _settingsUI.Show();
        }
    }

    private void Update()
    {
        BackButtonClick();
    }

    private void BackButtonClick()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isBackButtonEnabled && UIManager.Instance.UIStackPeek() is LobbyUI)
        {
            OpenExitUI();
        }
    }

    private IEnumerator EnableBackButton()
    {
        float enableBackButtonDelay = 0.25f;
        yield return new WaitForSeconds(enableBackButtonDelay);
        _isBackButtonEnabled = true;
    }
}