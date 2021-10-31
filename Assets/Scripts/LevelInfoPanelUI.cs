using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanelUI : MonoBehaviour
{
    private const string LevelKey = "Level";

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _biomesText;

    [Header("Images")]
    [SerializeField] private List<Image> _biomesImages;

    public void SetLevelInfoPanel(int levelNumber, List<BiomeData> currentBiomesData)
    {
        foreach (var item in currentBiomesData)
        {
            Debug.Log(item);
        }
        Debug.Log(currentBiomesData.Count);

        _levelText.text = $"{LocalizationManager.GetLocalizedText(LevelKey)}{levelNumber}";
        int biomesCounter = 0;
        string biomesText = "";

        foreach (var biomeData in currentBiomesData)
        {
            biomesText += $" - {LocalizationManager.GetLocalizedText(biomeData.BiomeName)}";
            _biomesImages[biomesCounter].sprite = biomeData.BiomeElement.ElementSpriteUI;
            biomesCounter++;
        }

        biomesText = biomesText.Remove(0, 3);
        _biomesText.text = biomesText;
    }

    private void Start()
    {
        LevelSpawner.Instance.OnLevelSpawned += SetLevelInfoPanel;
    }
}
