using System;
using System.Collections;
using System.Collections.Generic;
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
    public Button[] weaponBtns;
     private void Start()
     {
         StartCoroutine(cc());
         
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

         for (int i = 0; i < weaponBtns.Length; i++)
         {
             int index = i;
             weaponBtns[index].onClick.AddListener(() =>
             {
                 // Adjust the productIds index if it starts from 6
                 int productIdIndex = index + 7; // Assuming productIds starts from index 6
                 if (!PlayerPrefsData.IsWeaponPurchased(productIds[productIdIndex]))
                 {
                     BuyProduct(productIds[productIdIndex]);
                 }
             });
         }
     }  

     IEnumerator cc()
    {
        yield return new WaitForSeconds(3f);
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }
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

            if (product != null)
            {
                Debug.Log($"Purchasing 2: {product.definition.id}");
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
       // PlayerPrefsData.SaveProductId(productId);
        if (productId == "com.brawl.fish.laser" ||productId == "com.brawl.fish.legendkatana" ||productId == "com.brawl.fish.lightining" ||productId == "com.brawl.fish.poisedon" ||productId == "com.brawl.fish.sword" ||productId == "com.brawl.fish.umbrella")
        {
            PlayerPrefsData.SaveWeaponId(productId);
        }
        else
        {
            PlayerPrefsData.SaveProductId(productId);
        }
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

        foreach (var s in weaponBtns)
        {
            foreach (var p in PlayerPrefsData.GetAllPurchasedWeaponIds())
            {
                Debug.Log(p);
                if (s.GetComponent<WeaponDetailManager>().myID == p )
                {
                    s.GetComponent<WeaponDetailManager>().selectedButton.SetActive(false);
                    s.GetComponent<WeaponDetailManager>().selectedButton.GetComponent<TMP_Text>().text = "Selected";
                    s.GetComponent<WeaponDetailManager>().selectButton.SetActive(true);
                }  
            }
        }
    }
}
