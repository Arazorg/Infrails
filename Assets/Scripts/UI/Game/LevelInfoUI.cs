using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoUI : MonoBehaviour
{
    private const string LevelKey = "Level";

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _biomesText;

    [SerializeField] private List<Image> _levelsImages;
    [SerializeField] private float _showingTime;

    public void Show(int levelNumber, List<BiomeData> biomesData)
    {
        StartCoroutine(Showing(levelNumber, biomesData));
    }

    public void Hide()
    {
        StopAllCoroutines();
        GetComponent<AnimationsUI>().SetTransparency(0);
    }

    private IEnumerator Showing(int levelNumber, List<BiomeData> currentLevelsData)
    {
        float startDelay = 0.5f;
        yield return new WaitForSeconds(startDelay);

        GetComponent<AnimationsUI>().SetTransparency(1);
        _levelText.text = $"{LocalizationManager.GetLocalizedText(LevelKey)}{levelNumber}";
        int biomesCounter = 0;
        string biomesText = "";

        foreach (var levelData in currentLevelsData)
        {
            biomesText += $" - {LocalizationManager.GetLocalizedText(levelData.BiomeName)}";
            _levelsImages[biomesCounter].sprite = levelData.BiomeUISprite;
            biomesCounter++;
        }

        biomesText = biomesText.Remove(0, 3);
        _biomesText.text = biomesText;
        yield return new WaitForSecondsRealtime(_showingTime);

        GetComponent<AnimationsUI>().SetTransparency(0);
    }
}
