using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ProductData
{
    public List<string> productIds = new List<string>();
}
public class IAPDataHolder : SingletonMonoBehavier<IAPDataHolder>
{
    [SerializeField] internal int index;
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}
