using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDetailManager : MonoBehaviour
{
    [SerializeField] internal string fishName;
    [CanBeNull][SerializeField] internal string myID;
    [SerializeField] internal int id;
    [SerializeField] internal TMP_Text text;
    [SerializeField] private bool isFree;
    
    [Space] 
    [SerializeField] internal GameObject selectButton;
    [SerializeField] internal GameObject selectedButton;
    [SerializeField] internal GameObject selectBorder;
    
    

    private void Start()
    {
        
    }

    public void SetName()
    {
       text.text = fishName; 
    }

    public void SetIDAndFish()
    {
        if ((PlayerPrefsData.IsProductPurchased(myID) || isFree) && UIManager.Instance.isNull())
        { 
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.Instance.border = selectBorder.GetComponent<Image>();
            UIManager.Instance.selected = selectedButton;
            UIManager.Instance.select = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentIndex(id);
        }
        else if ((PlayerPrefsData.IsProductPurchased(myID) || isFree)&& !UIManager.Instance.isNull())
        {
            Debug.LogError("f");
            UIManager.Instance.border.enabled = false;
            UIManager.Instance.selected.SetActive(false);
            UIManager.Instance.select.SetActive(true);
            
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.Instance.border = selectBorder.GetComponent<Image>();
            UIManager.Instance.selected = selectedButton;
            UIManager.Instance.select = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentIndex(id);
        }
        
    }

    
}
