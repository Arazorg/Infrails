using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillInfoPanelUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image _passiveSkillImage;

    [Header("Texts")]
    [SerializeField] private LocalizedText _passiveSkillName;
    [SerializeField] private LocalizedText _passiveSkillDescription;

    private void Start()
    {
        if (CurrentGameInfo.Instance.PassiveSkillData != null)
            SetUI(CurrentGameInfo.Instance.PassiveSkillData);
    }

    public void SetUI(PassiveSkillData data)
    {
        GetComponent<AnimationsUI>().IsShowOnStart = true;
        _passiveSkillImage.sprite = data.ItemSpriteUI;
        _passiveSkillName.SetLocalization(data.ItemName);
        _passiveSkillName.GetComponent<TextMeshProUGUI>().color = data.ItemColor;
        _passiveSkillDescription.SetLocalization($"{data.ItemName}Description");
    }
}
