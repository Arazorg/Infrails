using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootInfoPanelUI : MonoBehaviour
{
    [SerializeField] private Image _lootImage;
    [SerializeField] private LocalizedText _lootNameText;
    [SerializeField] private LocalizedText _descriptionText;

    public void SetInfoPanel(ItemData itemData)
    {
        _lootImage.GetComponent<AnimationsUI>().Show();
        _lootImage.sprite = itemData.ItemSpriteUI;
        _lootNameText.GetComponent<TextMeshProUGUI>().color = itemData.ItemColor;
        _lootNameText.SetLocalization(itemData.ItemName);
        _descriptionText.SetLocalization(string.Format("{0}Description", itemData.ItemName));
    }
}
