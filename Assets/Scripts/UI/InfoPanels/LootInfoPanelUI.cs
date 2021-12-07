using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootInfoPanelUI : MonoBehaviour
{
    [Header("Info Panels")]
    [SerializeField] private WeaponInfoPanelUI _weaponInfoPanelUI;
    [SerializeField] private AmplificationInfoPanelUI _amplificationInfoPanelUI;

    [Header("Images")]
    [SerializeField] private Image _lootImage;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _lootNameText;
    [SerializeField] private LocalizedText _descriptionText;

    public void SetInfoPanel(ItemData lootData)
    {
        GetComponent<AnimationsUI>().Show();
        _lootImage.sprite = lootData.ItemSpriteUI;
        _lootNameText.GetComponent<TextMeshProUGUI>().color = lootData.ItemColor;
        _lootNameText.SetLocalization(lootData.ItemName);
        _descriptionText.SetLocalization(string.Format("{0}Description", lootData.ItemName));

        if (lootData is WeaponData)
            ShowWeaponLootInfo(lootData);
        else if (lootData is AmplificationData)
            ShowAmplificationsLootInfo(lootData);
        else
            _descriptionText.GetComponent<AnimationsUI>().Show();
    }

    private void ShowWeaponLootInfo(ItemData lootData)
    {
        _weaponInfoPanelUI.GetComponent<AnimationsUI>().Show();
        _weaponInfoPanelUI.SetPanelInfo(lootData);
        _descriptionText.GetComponent<AnimationsUI>().Show();
    }

    private void ShowAmplificationsLootInfo(ItemData lootData)
    {
        _amplificationInfoPanelUI.GetComponent<AnimationsUI>().Show();
        _amplificationInfoPanelUI.SetPanelInfo(lootData);
    }
}
