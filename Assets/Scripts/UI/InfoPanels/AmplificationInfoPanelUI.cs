using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmplificationInfoPanelUI : MonoBehaviour, IInfoPanel
{
    [Header("Images")]
    [SerializeField] private Image _image;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _nameText;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _bonusText;

    private AmplificationData _data;

    public void SetPanelInfo(ItemData itemData)
    {
        _data = itemData as AmplificationData;
        _image.sprite = _data.ItemSpriteUI;
        _nameText.GetComponent<TextMeshProUGUI>().color = itemData.ItemColor;
        _nameText.SetLocalization(_data.ItemName);
        _levelText.text = LocalizationManager.Instance.GetLocalizedText("AmplificationLevel") + (_data.Level).ToString();
        SetBonusText(_data);
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    public void DeleteAmplification()
    {
        GetComponent<AmplificationDeletePanelUI>().DeleteAmplification(_data);
        GetComponentInParent<AmplificationsUI>().HideAmplificationPanel();
    }

    private void SetBonusText(AmplificationData amplificationData)
    {
        int power = amplificationData.AmplificationPowers[amplificationData.Level - 1];
        string powerKey = amplificationData.CurrentAmplificationType.ToString();

        switch (amplificationData.CurrentAmplificationIncreaseType)
        {
            case AmplificationData.AmplificationIncreaseType.Percent:
                _bonusText.text = string.Format("+{0}% {1}", power, LocalizationManager.Instance.GetLocalizedText(powerKey));
                break;
            case AmplificationData.AmplificationIncreaseType.Add:
                _bonusText.text = string.Format("+{0} {1}", power, LocalizationManager.Instance.GetLocalizedText(powerKey));
                break;
        }

        _bonusText.color = amplificationData.ItemColor;
    }
}
