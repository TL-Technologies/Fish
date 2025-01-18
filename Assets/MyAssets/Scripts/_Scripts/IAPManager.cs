using System;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener
{
    
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;
    public string[] productIds;
    public Button[] purchaseBtns;
     private void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
        AssignButtonListeners();
        OnPurchaseRefreshUi();
    }
    private void AssignButtonListeners()
    {
        for (int i = 0; i < purchaseBtns.Length; i++)
        {
            int index = i;
            purchaseBtns[index].onClick.AddListener(() =>
            {
                if (!PlayerPrefsData.IsProductPurchased(productIds[index]))
                {
                    BuyProduct(productIds[index]);
                }
               
            });
        }
    }
    [Obsolete("Obsolete")]
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var productId in productIds)
        {
            Debug.Log(productId);
            builder.AddProduct(productId, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
        
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void BuyProduct(string productId)
    {
        if (IsInitialized())
        {
            Debug.LogError($"Purchasing : {productId}");
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
        }
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.LogError($"Initialization Success 3: {controller} {extensions}");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Initialization failed 1: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Initialization failed 2: {error} - {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        OnPurchaseSuccess(args.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase Failed: " + product.definition.id);
    }

    private void OnPurchaseSuccess(string productId)
    {
        Debug.Log("Purchase Success: " + productId);
        PlayerPrefsData.SaveProductId(productId);
        OnPurchaseRefreshUi();
    }

    void OnPurchaseRefreshUi()
    {
        foreach (var s in purchaseBtns)
        {
            foreach (var p in PlayerPrefsData.GetAllPurchasedProductIds())
            {
                Debug.Log(p);
                if (s.GetComponent<FishDetailManager>().myID == p )
                {
                 s.GetComponent<FishDetailManager>().selectedButton.SetActive(false);
                 s.GetComponent<FishDetailManager>().selectedButton.GetComponent<TMP_Text>().text = "Selected";
                 s.GetComponent<FishDetailManager>().selectButton.SetActive(true);
                }  
            }
            
        }
    }
}
