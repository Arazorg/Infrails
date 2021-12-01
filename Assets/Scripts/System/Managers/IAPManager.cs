using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private const string Coins25000 = "com.gd2d.infrails.coins25000";
    private const string Coins75000 = "com.gd2d.infrails.coins75000";
    private const string Coins200000 = "com.gd2d.infrails.coins200000";
    private const string Coins400000 = "com.gd2d.infrails.coins400000";
    private const string Trolley = "com.gd2d.infrails.trolley";

    private static IStoreController StoreController;
    private static IExtensionProvider StoreExtensionProvider;

    [SerializeField] private List<IAPButton> _IAPButtons;

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(Coins25000, ProductType.Consumable);
        builder.AddProduct(Coins75000, ProductType.Consumable);
        builder.AddProduct(Coins200000, ProductType.Consumable);
        builder.AddProduct(Coins400000, ProductType.Consumable);
        builder.AddProduct(Trolley, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyCoins25000()
    {
        BuyProductID(Coins25000);
    }

    public void BuyCoins75000()
    {
        BuyProductID(Coins75000);
    }

    public void BuyCoins200000()
    {
        BuyProductID(Coins200000);
    }

    public void BuyCoins400000()
    {
        BuyProductID(Coins400000);
    }

    public void BuyTrolleyForSupport()
    {
        BuyProductID(Trolley);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, Coins25000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerProgress.Instance.PlayerMoney += 25000;
        }
        else if (string.Equals(args.purchasedProduct.definition.id, Coins75000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerProgress.Instance.PlayerMoney += 75000;
        }
        else if (string.Equals(args.purchasedProduct.definition.id, Coins200000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerProgress.Instance.PlayerMoney += 200000;
        }
        else if (string.Equals(args.purchasedProduct.definition.id, Coins400000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerProgress.Instance.PlayerMoney += 400000;
        }
        else if (string.Equals(args.purchasedProduct.definition.id, Trolley, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerProgress.Instance.SupportDonate();
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        StoreController = controller;
        StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void Start()
    {
        if (StoreController == null)
        {
            InitializePurchasing();
            foreach (var button in _IAPButtons)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = GetPrice(button.productId);
            }
        }
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    private string GetPrice(string productID)
    {
        return StoreController.products.WithID(productID).metadata.localizedPriceString;
    }

    private bool IsInitialized()
    {
        return StoreController != null && StoreExtensionProvider != null;
    }
}
