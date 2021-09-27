using TMPro;
using UnityEngine;

public class CharacterInfoPanelUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _characterNameText;
    [SerializeField] private TextMeshProUGUI _characterDescriptionText;
    [SerializeField] private TextMeshProUGUI _characterHealthText;
    [SerializeField] private TextMeshProUGUI _characterArmorText;

    public void SetPanelInfo(CharacterData currentCharacterData)
    {
        string characterName = currentCharacterData.UnitName;
        _characterNameText.GetComponent<LocalizedText>().SetLocalization(characterName);
        _characterDescriptionText.GetComponent<LocalizedText>().SetLocalization(string.Format("{0}Description", characterName));
        _characterHealthText.text = currentCharacterData.MaxHealth.ToString();
        _characterArmorText.text = currentCharacterData.MaxArmor.ToString();
    }
}
