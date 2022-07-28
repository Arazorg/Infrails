using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationsUI))]
public class StartWeaponsPanelUI : MonoBehaviour
{
    [SerializeField] private List<StartWeaponInfoPanelUI> _weaponsPanels;
    [SerializeField] private AnimationsUI _nextButton;

    private WeaponData _selectedWeaponData;

    public WeaponData SelectedWeaponData => _selectedWeaponData;
    
    public void ShowWeapons(List<WeaponData> _weaponsData)
    {
        int counter = 0;
        foreach (var weaponPanel in _weaponsPanels)
        {
            weaponPanel.SetWeaponData(_weaponsData[counter]);
            var currentWeaponPanel = _weaponsPanels[(counter + 1) % _weaponsPanels.Count];
            weaponPanel.OnWeaponSelected += currentWeaponPanel.CancelWeapon;
            weaponPanel.OnWeaponSelected += SetWeaponData;
            counter++;
        }
        
        GetComponent<AnimationsUI>().Show();
    }

    public void Hide()
    {
        int counter = 0;
        foreach (var weaponPanel in _weaponsPanels)
        {
            var currentWeaponPanel = _weaponsPanels[(counter + 1) % _weaponsPanels.Count];
            weaponPanel.OnWeaponSelected -= currentWeaponPanel.CancelWeapon;
            weaponPanel.OnWeaponSelected -= SetWeaponData;
            counter++;
        }

        _nextButton.Hide();
        GetComponent<AnimationsUI>().Hide();
    }

    private void SetWeaponData(WeaponData weaponData)
    {
        _selectedWeaponData = weaponData;
        ShowNextButton();
    }

    private void ShowNextButton()
    {
        _nextButton.Show();
    }
}
