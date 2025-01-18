using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailManager : MonoBehaviour
{
    [SerializeField] internal string weaponName;
    [CanBeNull][SerializeField] internal string myID;
    [SerializeField] internal int id;
    [SerializeField] internal TMP_Text text;
    [SerializeField] private bool isFree;
    
    [Space] 
    [SerializeField] internal GameObject selectButton;
    [SerializeField] internal GameObject selectedButton;
    [SerializeField] internal GameObject selectBorder;
    
    
    public void SetName()
    {
        text.text = weaponName; 
    }
    
    public void SetIDAndWeapon()
    {
        if ((PlayerPrefsData.IsWeaponPurchased(myID) || isFree)&& UIManager.instance.isNullWeapon())
        { 
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.instance.borderWeapon = selectBorder.GetComponent<Image>();
            UIManager.instance.selectedWeapon = selectedButton;
            UIManager.instance.selectWeapon = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentWeaponIndex(id);
        }
        else if ((PlayerPrefsData.IsWeaponPurchased(myID) || isFree)&& !UIManager.instance.isNullWeapon())
        {
            UIManager.instance.borderWeapon.enabled = false;
            UIManager.instance.selectedWeapon.SetActive(false);
            UIManager.instance.selectWeapon.SetActive(true);
            
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.instance.borderWeapon = selectBorder.GetComponent<Image>();
            UIManager.instance.selectedWeapon = selectedButton;
            UIManager.instance.selectWeapon = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentWeaponIndex(id);
        }
        
    }
}
