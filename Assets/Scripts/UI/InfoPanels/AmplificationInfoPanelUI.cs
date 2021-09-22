using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmplificationInfoPanelUI : MonoBehaviour, IInfoPanel
{
    [SerializeField] private Image _image;
    [SerializeField] private LocalizedText _nameText;
    [SerializeField] private LocalizedText _powerTypeText;
    [SerializeField] private TextMeshProUGUI _levelValueText;
    [SerializeField] private TextMeshProUGUI _powerValueText;

    public void SetPanelInfo(ItemData itemData)
    {
        var amplificationData = itemData as AmplificationData;
        _image.sprite = amplificationData.ItemSpriteUI;
        _nameText.GetComponent<TextMeshProUGUI>().color = itemData.ItemColor;
        _nameText.SetLocalization(amplificationData.ItemName);
        _levelValueText.text = (amplificationData.Level).ToString();
        SetPowerText(amplificationData);
    }
    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    private void SetPowerText(AmplificationData amplificationData)
    {
        int power = amplificationData.AmplificationPowers[amplificationData.Level-1];

        switch (amplificationData.CurrentAmplificationIncreaseType)
        {
            case AmplificationData.AmplificationIncreaseType.Percent:
                _powerValueText.text = string.Format("+{0}%", power);
                break;
            case AmplificationData.AmplificationIncreaseType.Add:
                _powerValueText.text = string.Format("+{0}", power);
                break;
        }

        string powerKey = amplificationData.CurrentAmplificationType.ToString();
        _powerTypeText.SetLocalization(powerKey);
        _powerTypeText.GetComponent<TextMeshProUGUI>().color = amplificationData.ItemColor;
        _powerValueText.color = amplificationData.ItemColor;
    }
}
