using UnityEngine;

public class AmplificationsUI : BaseUI, IUIPanel
{
    [SerializeField] private CurrentAmplificationsPanelUI _currentAmplifications;
    [SerializeField] private AnimationsUI _currentAmplificationsAnimation;
    [SerializeField] private AnimationsUI _amplificationInfoPanel;

    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

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
        Close();
    }

    public void Open()
    {
        Show();
    }

    public void GoToGameShopLootUI()
    {
        UIManager.Instance.UIStackPop();
    }

    public void HideAmplificationPanel()
    {
        _currentAmplificationsAnimation.Show();
        _currentAmplifications.HideAmplificationInfo();
    }

    private void Close()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = false;
        Hide();
    }
}
