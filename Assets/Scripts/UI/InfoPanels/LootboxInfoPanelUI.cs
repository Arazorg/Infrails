using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootboxInfoPanelUI : MonoBehaviour
{
    [SerializeField] private Image _lootboxImage;
    [SerializeField] private TextMeshProUGUI _lootboxNameText;
    [SerializeField] private TextMeshProUGUI _lootboxDescriptionText;
    [SerializeField] private TextMeshProUGUI _lootboxPriceText;

    public void SetInfoPanel(LootboxData lootboxData)
    {
        _lootboxImage.sprite = lootboxData.MainSprite;
        _lootboxNameText.color = lootboxData.LootboxColor;
        _lootboxNameText.GetComponent<LocalizedText>().SetLocalization(lootboxData.NameKey);
        _lootboxDescriptionText.GetComponent<LocalizedText>().SetLocalization(string.Format("{0}Description", lootboxData.NameKey));
        _lootboxPriceText.text = lootboxData.Price.ToString();
    }
}
