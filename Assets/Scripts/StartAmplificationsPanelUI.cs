using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AnimationsUI))]
public class StartAmplificationsPanelUI : MonoBehaviour
{
    private const int MaxAmplificationsNumber = 3;

    [SerializeField] private RectTransform _amplificationsParentTransform;
    [SerializeField] private AmplificationImageUI _amplificationImageUIPrefab;
    [SerializeField] private AnimationsUI _playButton;
    [SerializeField] private List<TextMeshProUGUI> _amplificationsDescrtionsTexts;

    private List<AmplificationImageUI> _currentAmplificationsImages;
    private List<AmplificationImageUI> _selectedAmplificationsImages;
    private int _numberChangingAmplification;
    private int _numberSelectedAmplifications;

    public List<AmplificationImageUI> SelectedAmplificationsImages => _selectedAmplificationsImages;

    public void SpawnAmplifications(List<AmplificationData> _amplificationsData)
    {
        _selectedAmplificationsImages = new List<AmplificationImageUI>();
        _currentAmplificationsImages = new List<AmplificationImageUI>();
        foreach (var amplificationData in _amplificationsData)
        {
            var amplificationImage = Instantiate(_amplificationImageUIPrefab,
                _amplificationsParentTransform.position, Quaternion.identity,
                _amplificationsParentTransform);
            _currentAmplificationsImages.Add(amplificationImage);
            amplificationImage.Init(amplificationData);
            amplificationImage.OnAmplificationSelected += SetAmplification;
        }

        GetComponent<AnimationsUI>().Show();
    }

    public void Hide()
    {
        _playButton.Hide();
        GetComponent<AnimationsUI>().Hide();
    }

    public void UnfollowEvents()
    {
        foreach (var amplificationImage in _currentAmplificationsImages)
            amplificationImage.OnAmplificationSelected -= SetAmplification;
    }

    private void SetAmplification(AmplificationImageUI amplificationImageUI)
    {
        if (_numberSelectedAmplifications < MaxAmplificationsNumber)
        {
            _selectedAmplificationsImages.Add(amplificationImageUI);
            _amplificationsDescrtionsTexts[_numberSelectedAmplifications].text =
                LocalizationManager.Instance.GetLocalizedText(amplificationImageUI.AmplificationData.ItemName) 
                + "\n" + GetBonusText(amplificationImageUI.AmplificationData);
            _amplificationsDescrtionsTexts[_numberSelectedAmplifications].color = 
                amplificationImageUI.AmplificationData.ItemColor;
        }
        else
        {
            ChangeAmplification(amplificationImageUI);
        }

        _numberSelectedAmplifications++;
        if (_numberSelectedAmplifications >= MaxAmplificationsNumber)
            _numberSelectedAmplifications = MaxAmplificationsNumber;

        if (_numberSelectedAmplifications == MaxAmplificationsNumber)
            _playButton.Show();
    }

    private void ChangeAmplification(AmplificationImageUI amplificationImageUI)
    {
        _selectedAmplificationsImages[_numberChangingAmplification].RemoveAmplification();
        _selectedAmplificationsImages[_numberChangingAmplification] = amplificationImageUI;
        _amplificationsDescrtionsTexts[_numberChangingAmplification].text =
            LocalizationManager.Instance.GetLocalizedText(amplificationImageUI.AmplificationData.ItemName) 
            + "\n" + GetBonusText(amplificationImageUI.AmplificationData);
        _amplificationsDescrtionsTexts[_numberChangingAmplification].color = 
            amplificationImageUI.AmplificationData.ItemColor;
        _numberChangingAmplification = (_numberChangingAmplification + 1) % MaxAmplificationsNumber;
    }

    private string GetBonusText(AmplificationData amplificationData)
    {
        int levelsForAmplificationLevel = 10;
        amplificationData.Level = (PlayerProgress.Instance.LevelNumber / levelsForAmplificationLevel) + 1;
        int power = amplificationData.AmplificationPowers[amplificationData.Level - 1];
        string powerKey = amplificationData.CurrentAmplificationType.ToString();

        switch (amplificationData.CurrentAmplificationIncreaseType)
        {
            case AmplificationData.AmplificationIncreaseType.Percent:
                return string.Format("+{0}% {1}", power, LocalizationManager.Instance.GetLocalizedText(powerKey));

            case AmplificationData.AmplificationIncreaseType.Add:
                return string.Format("+{0} {1}", power, LocalizationManager.Instance.GetLocalizedText(powerKey));
        }
        
        return String.Empty;
    }
}