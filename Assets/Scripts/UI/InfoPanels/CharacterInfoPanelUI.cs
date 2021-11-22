using TMPro;
using UnityEngine;

public class CharacterInfoPanelUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private LocalizedText _characterNameText;
    [SerializeField] private LocalizedText _characterSkillNameText;
    [SerializeField] private LocalizedText _characterSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI _characterHealthText;
    [SerializeField] private TextMeshProUGUI _characterArmorText;

    public void SetPanelInfo(CharacterData currentCharacterData)
    {
        string characterName = currentCharacterData.UnitName;
        _characterNameText.SetLocalization(characterName);
        _characterSkillNameText.SetLocalization(string.Format("{0}Skill", characterName));
        _characterSkillDescriptionText.SetLocalization(string.Format("{0}SkillDescription", characterName));
        _characterHealthText.text = currentCharacterData.MaxHealth.ToString();
        _characterArmorText.text = currentCharacterData.MaxArmor.ToString();
    }
}
