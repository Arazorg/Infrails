using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameShopProductPanel : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private LocalizedText _nameText;
    [SerializeField] private LocalizedText _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;

    [Header("Images")]
    [SerializeField] private Image _productImage;

    public void SetInfo(GameShopProductData data)
    {
        _productImage.sprite = data.ProductSprite;
        _productImage.GetComponent<RectTransform>().sizeDelta = data.ProductSpriteSize;
        _nameText.SetLocalization(data.ProductName);
        _nameText.GetComponent<TextMeshProUGUI>().color = data.ProductColor;
        _descriptionText.SetLocalization($"{data.ProductName}Description");
        _priceText.text = data.Price.ToString();
    }
}
