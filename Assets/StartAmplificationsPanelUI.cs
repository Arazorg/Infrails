using System.Collections.Generic;
using UnityEngine;

public class StartAmplificationsPanelUI : MonoBehaviour
{
    private const int MaxAmplificationsNumber = 3;
    
    [SerializeField] private RectTransform _amplificationsParentTransform;
    [SerializeField] private AmplificationImageUI _amplificationImageUIPrefab;
    [SerializeField] private AnimationsUI _playButton;
    [SerializeField] private List<LocalizedText> _amplificationsDescrtionsTexts;
    
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
            amplificationImage.OnAmplificationClick += SetAmplification;
        }
    }

    public void UnfollowEvents()
    {
        foreach (var amplificationImage in _currentAmplificationsImages)
            amplificationImage.OnAmplificationClick -= SetAmplification;
    }
    
    private void SetAmplification(AmplificationImageUI amplificationImageUI)
    {
        if (_numberSelectedAmplifications < MaxAmplificationsNumber)
        {
            _selectedAmplificationsImages.Add(amplificationImageUI);
            _amplificationsDescrtionsTexts[_numberSelectedAmplifications]
                .SetLocalization(amplificationImageUI.AmplificationData.ItemName);
        }
        else
        {
            ChangeAmplification(amplificationImageUI);
        }

        _numberSelectedAmplifications++;
        if (_numberSelectedAmplifications >= MaxAmplificationsNumber)
            _numberSelectedAmplifications = MaxAmplificationsNumber;
        
        if(_numberSelectedAmplifications == MaxAmplificationsNumber)
            _playButton.Show();
    }

    private void ChangeAmplification(AmplificationImageUI amplificationImageUI)
    {
        _selectedAmplificationsImages[_numberChangingAmplification].RemoveAmplification();
        _selectedAmplificationsImages[_numberChangingAmplification] = amplificationImageUI;
        _amplificationsDescrtionsTexts[_numberChangingAmplification].SetLocalization(
            amplificationImageUI.AmplificationData.ItemName);
        _numberChangingAmplification = (_numberChangingAmplification + 1) % MaxAmplificationsNumber;
    }
}