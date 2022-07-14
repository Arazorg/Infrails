using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : BaseUI, IUIPanel
{
    [SerializeField] private List<AmplificationData> _currentAmplifications;
    [SerializeField] private List<WeaponData> _currentWeapons;
    [SerializeField] private StartAmplificationsPanelUI _startAmplificationsPanelUI;
    [SerializeField] private StartWeaponsPanelUI _startWeaponsPanel;

    private List<AmplificationData> _selectedAmplificationsData;
    private WeaponData _selectedWeaponData;
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


    public delegate void StartGamePanelClosed(List<AmplificationData> amplificationsData, WeaponData weaponData);

    public event StartGamePanelClosed OnStartGamePanelClosed;

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

    public void ShowWeaponsPanel()
    {
        _startAmplificationsPanelUI.Hide();
        _startWeaponsPanel.ShowWeapons(_currentWeapons);
    }

    public void GoToGame()
    {
        _startWeaponsPanel.Hide();
        SetAmplificationsData();
        OnStartGamePanelClosed?.Invoke(_selectedAmplificationsData, _startWeaponsPanel.SelectedWeaponData);
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
        _selectedAmplificationsData = new List<AmplificationData>();
        foreach (var amplificationImage in _startAmplificationsPanelUI.SelectedAmplificationsImages)
        {
            amplificationImage.AmplificationData.Level = 1;
            _selectedAmplificationsData.Add(amplificationImage.AmplificationData);
        }
    }
}