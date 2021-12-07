using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillPanelUI : MonoBehaviour
{
    private const string NoSkillLocalizationKey = "GetAmplificationsInShop";

    [SerializeField] private ShopMainPanelUI _shopMainPanelUI;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _skillNameText;
    [SerializeField] private LocalizedText _skillDescriptionText;

    [Header("Skill Panel")]
    [SerializeField] private Image _skillPanel;
    [SerializeField] private List<Image> _skillImages;
    [SerializeField] private List<Image> _skillLockImages;
    [SerializeField] private List<Sprite> _skillPanelSprites;

    [SerializeField] private AnimationsUI _goToShopButton;

    private CharacterData _currentCharacterData;

    public void Init(CharacterData currentCharacterData)
    {
        _currentCharacterData = currentCharacterData;
        SetSkillUI();
    }

    public void ChooseSkillPanel(int numberOfSkill)
    {
        if (PlayerProgress.Instance.GetPassiveSkillAvailability(_currentCharacterData.UnitName, 
            _currentCharacterData.CharacterPassiveSkills[numberOfSkill].ItemName))
        {
            CurrentGameInfo.Instance.PassiveSkillData = _currentCharacterData.CharacterPassiveSkills[numberOfSkill];
            _skillPanel.sprite = _skillPanelSprites[numberOfSkill];
            string skillName = _currentCharacterData.CharacterPassiveSkills[numberOfSkill].ItemName;
            _skillNameText.SetLocalization(skillName);
            _skillNameText.GetComponent<TextMeshProUGUI>().color = _currentCharacterData.CharacterPassiveSkills[numberOfSkill].ItemColor;
            _skillDescriptionText.SetLocalization(string.Format("{0}Description", skillName));
        }
    }

    public void GoToShop()
    {
        UIManager.Instance.UIStackPush(_shopMainPanelUI);
    }

    private void SetSkillUI()
    {
        _goToShopButton.Hide();
        _skillPanel.sprite = _skillPanelSprites[0];
        _skillDescriptionText.SetLocalization(NoSkillLocalizationKey);
        _skillNameText.SetEmptyText();
        for (int i = 0; i < _currentCharacterData.CharacterPassiveSkills.Count; i++)
            _skillImages[i].sprite = _currentCharacterData.CharacterPassiveSkills[i].ItemSpriteUI;

        CheckSkillsAvailability();
    }

    private void CheckSkillsAvailability()
    {
        int numberOfSkill = 0;
        int countOfAvailableSkills = 0;
        foreach (var skill in _currentCharacterData.CharacterPassiveSkills)
        {
            if (PlayerProgress.Instance.GetPassiveSkillAvailability(_currentCharacterData.UnitName, skill.ItemName))
            {
                _skillImages[numberOfSkill].enabled = true;
                _skillImages[numberOfSkill].raycastTarget = true;
                _skillLockImages[numberOfSkill].enabled = false;
                ChooseSkillPanel(numberOfSkill);
                countOfAvailableSkills++;
            }
            else
            {
                _skillImages[numberOfSkill].enabled = false;
                _skillImages[numberOfSkill].raycastTarget = false;
                _skillLockImages[numberOfSkill].enabled = true;
            }

            numberOfSkill++;
        }

        if (countOfAvailableSkills == 2)
            ChooseSkillPanel(0);
        else if (countOfAvailableSkills == 0)
            _goToShopButton.Show();
    }
}
