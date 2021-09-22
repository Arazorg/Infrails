using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LocalizationManager
{
    private static Dictionary<string, string> localizedText;
    private static string MissingTextString = "Localized text not found";
    private static GameObject[] texts;

    public static void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();

        TextAsset localizationFile = Resources.Load<TextAsset>(Path.Combine("LocalizationFiles/", fileName));
        string dataAsJson = localizationFile.text;

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
            
        RefreshText();
        SettingsInfo.Instance.CurrentLocalization = fileName;
        SettingsInfo.Instance.Save();
    }

    public static string GetLocalizedText(string key)
    {
        string result = MissingTextString;

        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public static List<string> GetLocalizedTextWithKey(string key)
    {
        List<string> textWithKey = new List<string>();
        foreach (var text in localizedText)
        {
            if(text.Key.ToString().Contains(key))
            {
                textWithKey.Add(text.Value);
            }
        }

        return textWithKey;
    }

    private static void RefreshText()
    {
        texts = GameObject.FindGameObjectsWithTag("Text");
        foreach (var text in texts)
        {
            LocalizedText localizedText = text.GetComponent<LocalizedText>();
            if (localizedText != null)
            {
                localizedText.SetLocalization();
            }  
        }
    }
}
