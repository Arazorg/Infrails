using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoPanelUI : MonoBehaviour, IInfoPanel
{
    [SerializeField] private Image _weaponImage;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _weaponNameText;
    [SerializeField] private LocalizedText _weaponDescriptionText;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _weaponDamageText;
    [SerializeField] private TextMeshProUGUI _weaponCritChanceText;
    [SerializeField] private TextMeshProUGUI _weaponFireRateText;

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    public void SetPanelInfo(ItemData itemData)
    {
        _weaponImage.sprite = itemData.ItemSpriteUI;
        _weaponNameText.SetLocalization(itemData.ItemName);
        _weaponNameText.GetComponent<TextMeshProUGUI>().color = itemData.ItemColor;
        _weaponDescriptionText.SetLocalization($"{itemData.ItemName}Description");

        var weaponData = itemData as WeaponData;
        _weaponDamageText.text = weaponData.Damage.ToString();
        _weaponCritChanceText.text = weaponData.CritChance.ToString();
        _weaponFireRateText.text = weaponData.FireRate.ToString();
    }
}
