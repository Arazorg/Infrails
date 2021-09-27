using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private TextMeshProUGUI _textValue;
    [SerializeField] private Type _type;

    private int _minValue, _maxValue;
    private float _currentPercent;

    public enum Type
    {
        Health,
        Armor
    }

    public void Init(Character character, int maxValue, int minValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        switch (_type)
        {
            case Type.Health:
                character.OnHealthChanged += SetValue;
                break;
            case Type.Armor:
                character.OnArmorChanged += SetValue;
                break;
        }

        SetValue(maxValue);
    }

    public void SetValue(int value)
    {
        _currentPercent = value / (float)(_maxValue - _minValue);
        _textValue.text = string.Format("{0} / {1}", value, _maxValue - _minValue);
        _barImage.fillAmount = _currentPercent;
    }
}
