using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private TextMeshProUGUI _textValue;
    [SerializeField] private Type _type;

    private int minValue, maxValue;
    private float currentPercent;

    public enum Type
    {
        Health,
        Armor
    }

    public void Init(Character character, int maxValue, int minValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
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
        currentPercent = value / (float)(maxValue - minValue);
        _textValue.text = string.Format("{0} / {1}", value, maxValue - minValue);
        _barImage.fillAmount = currentPercent;
    }
}
