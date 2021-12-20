using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAmplificationsPanelUI : MonoBehaviour
{
    [SerializeField] private List<Image> _amplificationsImages;
    [SerializeField] private AmplificationInfoPanelUI _amplificationInfoPanelUI;
    [SerializeField] private Sprite _emptyAmplificationSprite;

    private List<AmplificationData> _currentAmplificationsData;
    private CharacterAmplifications _characterAmplifications;

    public void Init(CharacterAmplifications characterAmplifications)
    {
        _currentAmplificationsData = new List<AmplificationData>();
        _characterAmplifications = characterAmplifications;
        _characterAmplifications.OnChangeAmplifications += SetAmplificationsUI;
    }

    public void ShowAmplificationInfo(int number)
    {
        if (number < _currentAmplificationsData.Count)
        {
            _amplificationInfoPanelUI.SetPanelInfo(_currentAmplificationsData[number]);
            _amplificationInfoPanelUI.GetComponent<AnimationsUI>().Show();
        }          
    }

    public void ShowAmplificationInfoWithHide(int number)
    {
        GetComponent<AnimationsUI>().Hide();
        if (number < _currentAmplificationsData.Count)
        {
            _amplificationInfoPanelUI.SetPanelInfo(_currentAmplificationsData[number]);
            _amplificationInfoPanelUI.GetComponent<AnimationsUI>().Show();
        }
    }

    public void HideAmplificationInfo()
    {
        _amplificationInfoPanelUI.GetComponent<AnimationsUI>().Hide();
    }

    public void DeleteAmplification(AmplificationData amplificationData)
    {
        _characterAmplifications.DeleteAmplification(amplificationData);
    }

    private void SetAmplificationsUI(List<AmplificationData> currentAmplificationsData)
    {
        _currentAmplificationsData = currentAmplificationsData;

        int counter = 0;
       
        foreach (var image in _amplificationsImages)
        {
            if (counter < currentAmplificationsData.Count)
                image.sprite = currentAmplificationsData[counter].ItemSpriteUI;
            else
                image.sprite = _emptyAmplificationSprite;

            counter++;
        }
    }
}
