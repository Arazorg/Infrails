using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    public void OnPlayerDead(int biomeNumber, string characterName)
    {
        Analytics.CustomEvent("Player Dead", new Dictionary<string, object>()
        {
            {"BiomeNumber", biomeNumber},
            {"Character Name", characterName},
        });
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
