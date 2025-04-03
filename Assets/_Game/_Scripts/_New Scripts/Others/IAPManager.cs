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
    public Button restoreBtn;

    private void Start()
    {
// Show restore button only on iOS
#if UNITY_IOS
        restoreBtn.gameObject.SetActive(true);
#else
    restoreBtn.gameObject.SetActive(false);
#endif
        restoreBtn.gameObject.SetActive(Application.platform == RuntimePlatform.IPhonePlayer);

       
        StartCoroutine(InitializeAfterDelay());
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
                int productIdIndex = index + 7;
                if (!PlayerPrefsData.IsWeaponPurchased(productIds[productIdIndex]))
                {
                    BuyProduct(productIds[productIdIndex]);
                }
            });
        }
        
        restoreBtn.AddCustomListner(RestorePurchases);
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.LogError("IAP not initialized!");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("Restoring purchases...");
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("Restore completed. Success: " + result);
            });
        }
        else
        {
            Debug.Log("Restore not supported on this platform.");
        }
    }

    private IEnumerator InitializeAfterDelay()
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
            builder.AddProduct(productId, ProductType.NonConsumable);
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
            Debug.Log($"Purchasing : --> {productId}");
            Product product = storeController.products.WithID(productId);

            if (product != null)
            {
                Debug.Log($"Purchasing 2 : ---> {product.definition.id}");
                storeController.InitiatePurchase(product);
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Initialization Success");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Initialization failed 1 -->: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Initialization failed 2 -->: {error} - {message}");
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

        if (IsWeapon(productId))
        {
            PlayerPrefsData.SaveWeaponId(productId);
            Debug.Log("Saving Weapon Id");
        }
        else
        {
            PlayerPrefsData.SaveProductId(productId);
            Debug.Log("Saving Product Id");
        }

        OnPurchaseRefreshUi();
    }

    private bool IsWeapon(string productId)
    {
        return productId == "com.brawl.laser" ||
               productId == "com.brawl.legendkatana" ||
               productId == "com.brawl.lightining" ||
               productId == "com.brawl.poisedon" ||
               productId == "com.brawl.sword" ||
               productId == "com.brawl.umbrella";
    }

    private void OnPurchaseRefreshUi()
    {
        Debug.Log("Called");
        foreach (var s in purchaseBtns)
        {
            foreach (var p in PlayerPrefsData.GetAllPurchasedProductIds())
            {
                Debug.Log("Called--> " + p);
                if (s.GetComponent<FishDetailManager>().myID == p)
                {
                    Debug.Log("Called--> " + p);
                    var fishManager = s.GetComponent<FishDetailManager>();
                    fishManager.selectedButton.SetActive(false);
                    fishManager.selectedButton.GetComponent<TMP_Text>().text = "Selected";
                    fishManager.selectButton.SetActive(true);
                    Debug.Log("hereeee");
                }
            }
        }

        foreach (var s in weaponBtns)
        {
            foreach (var p in PlayerPrefsData.GetAllPurchasedWeaponIds())
            {
                Debug.Log("Called--> " + p);
                if (s.GetComponent<WeaponDetailManager>().myID == p)
                {
                    var weaponManager = s.GetComponent<WeaponDetailManager>();
                    weaponManager.selectedButton.SetActive(false);
                    weaponManager.selectedButton.GetComponent<TMP_Text>().text = "Selected";
                    weaponManager.selectButton.SetActive(true);
                    Debug.Log("hereeee");
                }
            }
        }
    }
}
