using System.IO;
using UnityEngine;

public class SaveSystem
{
    public void Save(string json, string filePath)
    {
        filePath = $"{Application.persistentDataPath}/{filePath}";
        using var writer = new StreamWriter(filePath);
        writer.WriteLine(json);
    }

    public string Load(string filePath)
    {
        filePath = $"{Application.persistentDataPath}/{filePath}";
        if (File.Exists(filePath))
        {
            string json = string.Empty;
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    json += line;
                }
            }
            if (string.IsNullOrEmpty(json))
            {
                return string.Empty;
            }

            return json;
        }
        return string.Empty;
    }
}
