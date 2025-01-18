
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerPrefsData
{
    #region PlayerData

    public static void SaveName(string name)
    {
        PlayerPrefs.SetString("name", name);
    }

    public static string GetName()
    {
        return PlayerPrefs.GetString("name");
    }

    #endregion
    
    #region Extension
    public static void AddCustomListner(this Button btn, Action onComplete)
    {
        if (btn == null)
        {
            Debug.LogError("Assigned button is null");
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { onComplete(); });
    }
    
    #endregion
    
    #region Sound And Haptic
    
    public static bool musicTurn
    {
        get
        {
            return PlayerPrefs.GetInt($"musicTurn", 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt($"musicTurn", value ? 0 : 1);
            PlayerPrefs.Save();
        }
    }
    public static bool vibrate
    {
        get
        {
            return PlayerPrefs.GetInt($"Vibrate", 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt($"Vibrate", value ? 0 : 1);
            PlayerPrefs.Save();
        }
    }
    public static bool soundTurn
    {
        get
        {
            return PlayerPrefs.GetInt($"soundTurn", 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt($"soundTurn", value ? 0 : 1);
            PlayerPrefs.Save();
        }
    }
    
    #endregion
    
    private const string ProductIdsKey = "ProductIds";

    public static void SaveProductId(string productId)
    {
        ProductData productData = LoadAllProductIds();
        if (!productData.productIds.Contains(productId))
        {
            productData.productIds.Add(productId);
        }
        string jsonData = JsonUtility.ToJson(productData);
        PlayerPrefs.SetString(ProductIdsKey, jsonData);
        PlayerPrefs.Save();
    }

    public static ProductData LoadAllProductIds()
    {
        if (PlayerPrefs.HasKey(ProductIdsKey))
        {
            string jsonData = PlayerPrefs.GetString(ProductIdsKey);
            return JsonUtility.FromJson<ProductData>(jsonData);
        }
        return new ProductData();
    }

    public static bool IsProductPurchased(string productId)
    {
        ProductData productData = LoadAllProductIds();
        return productData.productIds.Contains(productId);
    }
    
    public static List<string> GetAllPurchasedProductIds()
    {
        ProductData productData = LoadAllProductIds();
        return productData.productIds;
    }


    public static void SetCurrentIndex( int index)
    {
        PlayerPrefs.SetInt("currentIndex", index);
    }

    public static int GetCurrentIndex()
    {
        return PlayerPrefs.GetInt("currentIndex");
    }
}
