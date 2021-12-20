using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private readonly string MissingTextString = "Localized text not found";

    private Dictionary<string, string> _localizedTexts;

    public delegate void LoadLozalization();
    public event LoadLozalization OnLoadLozalization;

    public void LoadLocalization(string fileName)
    {
        _localizedTexts = new Dictionary<string, string>();

        TextAsset localizationFile = Resources.Load<TextAsset>(Path.Combine("LocalizationFiles/", fileName));
        string dataAsJson = localizationFile.text;

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            _localizedTexts.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
            
        RefreshText();
        SettingsInfo.Instance.CurrentLocalization = fileName;
        SettingsInfo.Instance.Save();
    }

    public string GetLocalizedText(string key)
    {
        string result = MissingTextString;

        if (_localizedTexts.ContainsKey(key))
            result = _localizedTexts[key];

        return result;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }          
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }    
            
    }

    private void RefreshText()
    {
        OnLoadLozalization?.Invoke();
    }
}
