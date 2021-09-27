using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AvailableItemsUI : BaseUI, IUIPanel
{
    private const string YourWeaponKey = "YourWeapon";
    private const string YourAmplificationsKey = "YourAmplifications";

    [SerializeField] private List<GameObject> _infoPanelsPrefabs;
    [SerializeField] private ShopMainPanelUI _shopMainPanelUI;
    [SerializeField] private RectTransform _content;
    [SerializeField] private LocalizedText _itemsText;
    [SerializeField] private float _infoPanelHeight;

    private GameObject _infoPanelPrefab;
    private List<IInfoPanel> _infoPanels = new List<IInfoPanel>();
    private TypeOfPanel _typeOfPanel;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public enum TypeOfPanel
    {
        Amplification,
        Weapon
    }

    public void OnPush()
    {
        Open();
    }

    public void OnPop()
    {
        Hide();
    }

    public void OnShow()
    {
        Open();
    }

    public void OnHide()
    {
        Hide();
    }

    public void Close()
    {
        UIManager.Instance.UIStackPop();
    }

    public void Init(TypeOfPanel typeOfPanel)
    {
        _typeOfPanel = typeOfPanel;

        switch (typeOfPanel)
        {
            case TypeOfPanel.Amplification:
                _infoPanelPrefab = _infoPanelsPrefabs[0];
                _itemsText.SetLocalization(YourAmplificationsKey);
                break;
            case TypeOfPanel.Weapon:
                _infoPanelPrefab = _infoPanelsPrefabs[1];
                _itemsText.SetLocalization(YourWeaponKey);
                break;
        }

        AddItemsToUI();
    }

    public void OpenShop()
    {
        UIManager.Instance.UIStackPush(_shopMainPanelUI);
    }

    private void Open()
    {
        _isBackButtonEnabled = true;
        _isPopAvailable = true;
        Show();
    }

    private void AddItemsToUI()
    {
        DestroyPreviousPanels();
        List<ItemData> itemsData = GetItemsData();
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, itemsData.Count * _infoPanelHeight);

        foreach (var data in itemsData)
        {
            var infoPanel = Instantiate(_infoPanelPrefab, _content).GetComponent<IInfoPanel>();
            infoPanel.SetPanelInfo(data);
            _infoPanels.Add(infoPanel);
        }

        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, -_content.sizeDelta.y);
    }

    private void DestroyPreviousPanels()
    {
        foreach (var infoPanel in _infoPanels)
            infoPanel.DestroyPanel();

        _infoPanels.Clear();
    }

    private List<ItemData> GetItemsData()
    {
        switch (_typeOfPanel)
        {
            case TypeOfPanel.Amplification:
                return PlayerProgress.Instance.GetAmplificationsData(GameConstants.Available).Cast<ItemData>().ToList();
            case TypeOfPanel.Weapon:
                return PlayerProgress.Instance.GetWeaponsData(GameConstants.Available).Cast<ItemData>().ToList();
            default:
                return null;
        }
    }
}
