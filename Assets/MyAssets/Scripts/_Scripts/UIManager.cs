using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [Header("Username")]
    [SerializeField] internal TMP_InputField usernameInput;
    [SerializeField] internal Button doneButton;
    
    [Header("Settings")]
    [SerializeField] internal GameObject settingPrefab;
    [SerializeField] internal Button settingButton;

    [Header("All bordersn and extras")]
    [SerializeField] internal Image border;
    [SerializeField] internal GameObject selected;
    [SerializeField] internal GameObject select; 
    
    
    [Header("All bordersn and extras")]
    [SerializeField] internal Image borderWeapon;
    [SerializeField] internal GameObject selectedWeapon;
    [SerializeField] internal GameObject selectWeapon;

    [Space][Header("Shop ")] 
    [SerializeField] internal Button ShopBtn;
    [SerializeField] internal Button shopClose;
    [SerializeField] internal GameObject shop;
    
    [Space]
    [SerializeField] List<FishDetailManager> fishList = new List<FishDetailManager>();
    [SerializeField] List<WeaponDetailManager> weaponList = new List<WeaponDetailManager>();

    [Space] public GameObject objShop;
    [Space] public TMP_Text fishText;
    [Space] public GameObject objWeapon;
    [Space] public TMP_Text weaponText;
    public Color normalColor;
    public Color disabledColor;


    public bool isNull()
    {
        if (border == null && selected == null && select == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool isNullWeapon()
    {
        if (borderWeapon == null && selectedWeapon == null && selectWeapon == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetNameAtStart();
        FishAtStart();
        WeaponAtStart();
        doneButton.AddCustomListner(OnClickDone);
        settingButton.AddCustomListner(ShowSetting);
        ShopBtn.AddCustomListner(Openshop);
        shopClose.AddCustomListner(CloseShop);
    }

    private void Openshop()
    {
      shop.SetActive(true);
    }

    private void CloseShop()
    {
        shop.SetActive(false);
    }

    #endregion
    
    
    #region Private Methods
    
    private void OnClickDone()
    {
        if (!string.IsNullOrEmpty(usernameInput.text) && usernameInput.text.Length >= 1 && usernameInput.text.Length <= 25)
        {
            var name = usernameInput.text;
           PlayerPrefsData.SaveName(name);
        }
    }

    private void SetNameAtStart()
    {
        if (string.IsNullOrEmpty(PlayerPrefsData.GetName()))
        {
            usernameInput.text = "PLAYER0921";
            PlayerPrefsData.SaveName(usernameInput.text);
        }
        else
        {
            usernameInput.text = PlayerPrefsData.GetName();
        }
    }

    private void ShowSetting()
    {
        SettingBox.Setup();
    }


    void FishAtStart()
    {
        int fishIndex = PlayerPrefsData.GetCurrentIndex() - 1;
        if (fishIndex < 0) fishIndex = 0; // Ensure index is not negative

        fishList[fishIndex].selectBorder.GetComponent<Image>().enabled = true;
        fishList[fishIndex].selectedButton.SetActive(true);
        fishList[fishIndex].selectButton.SetActive(false);

        border = fishList[fishIndex].selectBorder.GetComponent<Image>();
        selected = fishList[fishIndex].selectedButton;
        select = fishList[fishIndex].selectButton;
    }

    void WeaponAtStart()
    {
        int weaponIndex = PlayerPrefsData.GetCurrentWeaponIndex()-1;
        if (weaponIndex < 0) weaponIndex = 0; // Ensure index is not negative

        weaponList[weaponIndex].selectBorder.GetComponent<Image>().enabled = true;
        weaponList[weaponIndex].selectedButton.SetActive(true);
        weaponList[weaponIndex].selectButton.SetActive(false);

        borderWeapon = weaponList[weaponIndex].selectBorder.GetComponent<Image>();
        selectedWeapon = weaponList[weaponIndex].selectedButton;
        selectWeapon = weaponList[weaponIndex].selectButton;
    }

    #endregion


    public void OpenFish()
    {
        objShop.SetActive(true);
        fishText.color = normalColor;
        weaponText.color = disabledColor;
        objWeapon.SetActive(false);
    }  
    
    public void OpenSword()
    {
        objShop.SetActive(false);
        fishText.color = disabledColor;
        weaponText.color = normalColor;
        objWeapon.SetActive(true);
    }
    
}
