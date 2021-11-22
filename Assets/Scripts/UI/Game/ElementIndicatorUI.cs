using UnityEngine;
using UnityEngine.UI;

public class ElementIndicatorUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image _elementOutlineImage;
    [SerializeField] private Image _elementImage;

    private Element _element;

    public void Init(CharacterWeapon characterWeapon)
    {
        characterWeapon.OnElementChanged += SetElementImage;
        LevelSpawner.Instance.OnBiomeSpawned += SetElementOutlineImage; 
        SetElementImage(LevelSpawner.Instance.CurrentBiomeData.BiomeElement);
    }

    public void SetElementImage(Element element)
    {
        _element = element;
        _elementImage.sprite = element.ElementSpriteUI;
        SetElementOutlineImage();
    }

    private void SetElementOutlineImage()
    {
        _elementOutlineImage.color = Color.white;
        float interaction = LevelSpawner.Instance.CurrentBiomeData.BiomeElement.GetElementInteractionByType(_element.ElementType);

        if (interaction < 1)
            _elementOutlineImage.color = Color.red;
        else if (interaction > 1)
            _elementOutlineImage.color = Color.green;
    }
}
