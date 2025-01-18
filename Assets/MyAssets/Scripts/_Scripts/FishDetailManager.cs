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
        if ((PlayerPrefsData.IsProductPurchased(myID) || isFree)&& UIManager.instance.isNull())
        { 
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.instance.border = selectBorder.GetComponent<Image>();
            UIManager.instance.selected = selectedButton;
            UIManager.instance.select = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentIndex(id);
        }
        else if ((PlayerPrefsData.IsProductPurchased(myID) || isFree)&& !UIManager.instance.isNull())
        {
            UIManager.instance.border.enabled = false;
            UIManager.instance.selected.SetActive(false);
            UIManager.instance.select.SetActive(true);
            
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.instance.border = selectBorder.GetComponent<Image>();
            UIManager.instance.selected = selectedButton;
            UIManager.instance.select = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentIndex(id);
        }
        
    }

    
}
