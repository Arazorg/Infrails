using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : BaseUI, IUIPanel
{
    [SerializeField] private StartAmplificationsPanelUI _startAmplificationsPanelUI;
    [SerializeField] private StartWeaponsPanelUI _startWeaponsPanel;

    private List<AmplificationData> _currentAmplifications;
    private List<WeaponData> _currentWeapons;
    private List<AmplificationData> _selectedAmplificationsData;
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

    public void SetWeapons(List<WeaponData> weaponsData)
    {
        _currentWeapons = weaponsData;
    }

    public void SetAmplifications(List<AmplificationData> amplificationsData)
    {
        _currentAmplifications = amplificationsData;
        //Open();
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
        Close();
    }

    public void ShowAmplificaiontsPanel()
    {
        _startWeaponsPanel.Hide();
        _startAmplificationsPanelUI.SpawnAmplifications(_currentAmplifications);
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
        _startWeaponsPanel.ShowWeapons(_currentWeapons);      
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