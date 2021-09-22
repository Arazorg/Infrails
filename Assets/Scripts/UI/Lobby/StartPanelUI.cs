using UnityEngine;

public class StartPanelUI : BaseUI, IUIPanel
{
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private TutorialUI _tutorialUI;

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
        Hide();
    }

    private void Open()
    {
        _isBackButtonEnabled = false;

        if (!GameStates.isOpen)
        {
            Show();
        }
        else
        {
            
        }
    }

    private void Close()
    {
        UIManager.Instance.UIStackPush(_lobbyUI);
    }
}
