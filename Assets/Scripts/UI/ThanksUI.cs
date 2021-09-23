using UnityEngine;

public class ThanksUI : BaseUI, IUIPanel
{
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }
    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }
    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public void OnHide()
    {
        Hide();
    }

    public void OnPop()
    {
        Hide();
    }

    public void OnPush()
    {
        Open();
    }

    public void OnShow()
    {
        Open();
    }
    public void Close()
    {
        UIManager.Instance.UIStackPop();
    }

    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }

    private void Open()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        Show();
    }
}
