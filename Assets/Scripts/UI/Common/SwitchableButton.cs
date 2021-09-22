using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchableButton : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private string _onLocalizationKey;
    [SerializeField] private string _offLocalizationKey;
    [SerializeField] private bool _isSetText;

    private Image image;

    public void SetButtonState(bool isState)
    {
        if (isState)
        {
            image.sprite = _onSprite;
        }                   
        else
        {
            image.sprite = _offSprite;
        }
            
        if (_isSetText)
        {
            SetText(isState);
        }
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void SetText(bool isState)
    {
        LocalizedText localizedText = GetComponentInChildren<LocalizedText>();

        if (localizedText != null)
        {
            if (isState)
            {
                localizedText.SetLocalization(_onLocalizationKey);
            }
            else
            {
                localizedText.SetLocalization(_offLocalizationKey);
            }
        }       
    }
}
