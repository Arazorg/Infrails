using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItemsPanelUI : MonoBehaviour
{
    [Header("UI Scripts")]
    [SerializeField] private WeaponInfoPanelUI _weaponInfoPanelUI;

    [Header("Images")]
    [SerializeField] private Image _weaponImage;
    [SerializeField] private List<Image> _amplificationsImage;

    [Header("Texts")]
    [SerializeField] private LocalizedText _weaponNameText;

    [Header("Animations UI")]
    [SerializeField] private AnimationsUI _shopItemAnimation;
    [SerializeField] private AnimationsUI _weaponInfoPanelAnimation;

    [Header("Sprites")]
    [SerializeField] private Sprite _emptyAmplificationSprite;

    private WeaponData _weaponData;

    public void SetItems(WeaponData weaponData, List<AmplificationData> amplificationsData)
    {
        SetWeapon(weaponData);
        SetAmplificationsPanel(amplificationsData);
    }

    public void ShowWeaponInfoPanel()
    {
        _weaponInfoPanelAnimation.Show();
        _shopItemAnimation.Hide();
    }

    public void HideWeaponInfoPanel()
    {
        _weaponInfoPanelAnimation.Hide();
        _shopItemAnimation.Show();
    }

    private void SetWeapon(WeaponData weaponData)
    {
        _weaponData = weaponData;
        _weaponImage.sprite = weaponData.ItemSpriteUI;
        _weaponNameText.GetComponent<LocalizedText>().SetLocalization(weaponData.ItemName);
        _weaponInfoPanelUI.SetPanelInfo(_weaponData);
    }

    private void SetAmplificationsPanel(List<AmplificationData> amplificationsData)
    {
        int amplificationsCounter = 0;
        foreach (var image in _amplificationsImage)
        {
            image.sprite = _emptyAmplificationSprite;

            if (amplificationsData.Count != 0)
            {
                if (amplificationsData[amplificationsCounter] != null)
                    image.sprite = amplificationsData[amplificationsCounter].ItemSpriteUI;
            }

            amplificationsCounter++;
        }
    }
}
