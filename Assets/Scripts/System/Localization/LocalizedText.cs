using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    private const string EmptyKey = "Empty";

    [SerializeField] private string key;

    private TextMeshProUGUI _text;

    public void SetLocalization(string _key = "")
    {
        if (_key != string.Empty)
            key = _key;

        if (_text == null)
            SetTextComponent();

        _text.text = LocalizationManager.Instance.GetLocalizedText(key);
    }

    public void SetEmptyText()
    {
        _text.text = LocalizationManager.Instance.GetLocalizedText(EmptyKey);
    }

    private void Start()
    {
        SetTextComponent();
    }

    private void SetTextComponent()
    {
        if (TryGetComponent(out TextMeshProUGUI text))
            _text = text;
    }
}
