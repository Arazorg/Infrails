using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : BaseUI, IUIPanel
{
    [SerializeField] private List<AmplificationData> _currentAmplifications;
    [SerializeField] private StartAmplificationsPanelUI _startAmplificationsPanelUI;

    private List<AmplificationData> _selectedAmplifications;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public bool IsBackButtonEnabled
    {
        get => _isBackButtonEnabled;
        set => _isBackButtonEnabled = value;
    }

    public bool IsPopAvailable
    {
        get => _isPopAvailable;
        set => _isPopAvailable = value;
    }


    public delegate void AmplificationsSelected(List<AmplificationData> amplificationsData);

    public event AmplificationsSelected OnAmplificationsSelected;

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

    public void GoToGame()
    {
        SetAmplificationsData();
        OnAmplificationsSelected?.Invoke(_selectedAmplifications);
    }

    private void Open()
    {
        Time.timeScale = 0f;
        _isBackButtonEnabled = false;
        _isPopAvailable = false;
        _startAmplificationsPanelUI.SpawnAmplifications(_currentAmplifications);
        Show();
    }

    private void Close()
    {
        Time.timeScale = 1f;
        _startAmplificationsPanelUI.UnfollowEvents();
        Hide();
    }

    private void SetAmplificationsData()
    {
        _selectedAmplifications = new List<AmplificationData>();
        foreach (var amplificationImage in _startAmplificationsPanelUI.SelectedAmplificationsImages)
        {
            amplificationImage.AmplificationData.Level = 1;
            _selectedAmplifications.Add(amplificationImage.AmplificationData);
        }
    }
}