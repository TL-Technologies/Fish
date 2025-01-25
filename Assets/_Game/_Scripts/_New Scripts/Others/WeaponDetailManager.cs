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
        if ((PlayerPrefsData.IsWeaponPurchased(myID) || isFree)&& UIManager.Instance.isNullWeapon())
        { 
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.Instance.borderWeapon = selectBorder.GetComponent<Image>();
            UIManager.Instance.selectedWeapon = selectedButton;
            UIManager.Instance.selectWeapon = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentWeaponIndex(id);
        }
        else if ((PlayerPrefsData.IsWeaponPurchased(myID) || isFree)&& !UIManager.Instance.isNullWeapon())
        {
            UIManager.Instance.borderWeapon.enabled = false;
            UIManager.Instance.selectedWeapon.SetActive(false);
            UIManager.Instance.selectWeapon.SetActive(true);
            
            selectBorder.GetComponent<Image>().enabled = true;
            selectButton.SetActive(false);
            selectedButton.SetActive(true);
            
            UIManager.Instance.borderWeapon = selectBorder.GetComponent<Image>();
            UIManager.Instance.selectedWeapon = selectedButton;
            UIManager.Instance.selectWeapon = selectButton;
            IAPDataHolder.Instance.index = id;
            PlayerPrefsData.SetCurrentWeaponIndex(id);
        }
        
    }
}
