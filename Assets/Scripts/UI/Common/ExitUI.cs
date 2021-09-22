using UnityEngine;

public class ExitUI : BaseUI, IUIPanel
{
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
        Hide();
    }

    public void OnShow()
    {
        Show();
    }

    public void OnHide()
    {
        Hide();
    }

    public void Open()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        Show();
    }

    public void Close()
    {
        UIManager.Instance.UIStackPop();
    }

    public void GoToLobby()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.Lobby);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
