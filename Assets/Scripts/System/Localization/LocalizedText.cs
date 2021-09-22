using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    private const string EmptyKey = "Empty";

    [SerializeField] private string key;
    [SerializeField] private bool isLocalizeOnStart;

    private TextMeshProUGUI textUI;
    private TextMeshPro text;

    private bool isTextUI;

    void Start()
    {
        GetTextComponent();
    }

    public void SetLocalization(string _key = "")
    {
        if (_key != string.Empty)
            key = _key;
        if (text == null && textUI == null)
            GetTextComponent();
        if (isTextUI)
        {
            textUI.text = LocalizationManager.GetLocalizedText(key);
        }
        else
        {
            text.text = LocalizationManager.GetLocalizedText(key);
        }
    }

    public void SetEmptyText()
    {
        textUI.text = LocalizationManager.GetLocalizedText(EmptyKey);
    }

    private void GetTextComponent()
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            textUI = GetComponent<TextMeshProUGUI>();
            isTextUI = true;
        }
        if (GetComponent<TextMeshPro>() != null)
        {
            text = GetComponent<TextMeshPro>();
            isTextUI = false;
        }
        if (isLocalizeOnStart)
            SetLocalization();
    }
}
