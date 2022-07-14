using UnityEngine;

public class StartWeaponInfoPanelUI : MonoBehaviour
{
    [SerializeField] private WeaponInfoPanelUI _weaponInfoPanelUI;
    [SerializeField] private AnimationsUI _selectImage;

    private WeaponData _currentWeaponData;
    
    public delegate void WeaponSelected(WeaponData weaponData);

    public event WeaponSelected OnWeaponSelected;

    public WeaponData CurrentWeaponData => _currentWeaponData;

    public void SetWeaponData(WeaponData weaponData)
    {
        _currentWeaponData = weaponData;
       _weaponInfoPanelUI.SetPanelInfo(weaponData);
    }

    public void SelectWeapon()
    {
        _selectImage.Show();
        OnWeaponSelected?.Invoke(_currentWeaponData);
    }

    public void CancelWeapon(WeaponData weaponData)
    {
        _selectImage.Hide();
    }
}