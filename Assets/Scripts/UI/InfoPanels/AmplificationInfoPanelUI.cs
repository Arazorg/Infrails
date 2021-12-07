using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmplificationInfoPanelUI : MonoBehaviour, IInfoPanel
{
    [Header("Images")]
    [SerializeField] private Image _image;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _nameText;
    [SerializeField] private LocalizedText _powerTypeText;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelValueText;
    [SerializeField] private TextMeshProUGUI _powerValueText;

    private AmplificationData _amplificationData;

    public void SetPanelInfo(ItemData itemData)
    {
        _amplificationData = itemData as AmplificationData;
        _image.sprite = _amplificationData.ItemSpriteUI;
        _nameText.GetComponent<TextMeshProUGUI>().color = itemData.ItemColor;
        _nameText.SetLocalization(_amplificationData.ItemName);
        _levelValueText.text = (_amplificationData.Level).ToString();
        SetPowerText(_amplificationData);
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    public void DeleteAmplification()
    {
        GetComponent<AmplificationDeletePanelUI>().DeleteAmplification(_amplificationData);
        GetComponentInParent<AmplificationsUI>().HideAmplificationPanel();
    }

    private void SetPowerText(AmplificationData amplificationData)
    {
        int power = amplificationData.AmplificationPowers[amplificationData.Level - 1];

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
