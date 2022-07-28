using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimationsUI))]
public class LevelInfoPanelUI : MonoBehaviour
{
    private const string LevelKey = "Level";

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _biomesText;

    [Header("Images")]
    [SerializeField] private List<Image> _biomesImages;

    public void Show()
    {
        StartCoroutine(Showing());
    }

    public void SetLevelInfoPanel(int levelNumber, List<BiomeData> currentBiomesData)
    {
        _levelText.text = $"{LocalizationManager.Instance.GetLocalizedText(LevelKey)}{levelNumber}";
        int biomesCounter = 0;
        string biomesText = "";

        foreach (var image in _biomesImages)
            image.gameObject.SetActive(false);

        foreach (var biomeData in currentBiomesData)
        {
            biomesText += $" - {LocalizationManager.Instance.GetLocalizedText(biomeData.BiomeName)}";
            _biomesImages[biomesCounter].gameObject.SetActive(true);
            _biomesImages[biomesCounter].sprite = biomeData.BiomeElement.ElementSpriteUI;
            biomesCounter++;
        }

        biomesText = biomesText.Remove(0, 3);

        string biomesKey = "Biomes";
        _biomesText.text = LocalizationManager.Instance.GetLocalizedText(biomesKey) + biomesText;
    }

    private void Start()
    {
        LevelSpawner.Instance.OnLevelSpawned += SetLevelInfoPanel;
    }

    private IEnumerator Showing()
    {
        float startDelay = 0.75f;
        yield return new WaitForSeconds(startDelay);
        GetComponent<AnimationsUI>().Show();

        float hideDelay = 2.5f;
        yield return new WaitForSeconds(hideDelay);
        GetComponent<AnimationsUI>().Hide();
    }
}
