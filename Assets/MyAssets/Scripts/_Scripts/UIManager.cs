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

    [Space][Header("Shop ")] 
    [SerializeField] internal Button ShopBtn;
    [SerializeField] internal Button shopClose;
    [SerializeField] internal GameObject shop;
    
    [Space]
    [SerializeField] List<FishDetailManager> fishList = new List<FishDetailManager>();


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

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetNameAtStart();
        AtStart();
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


    void AtStart()
    {
        fishList[PlayerPrefsData.GetCurrentIndex()].selectBorder.GetComponent<Image>().enabled = true;
        fishList[PlayerPrefsData.GetCurrentIndex()].selectedButton.SetActive(true);
        fishList[PlayerPrefsData.GetCurrentIndex()].selectButton.SetActive(false);

        border = fishList[PlayerPrefsData.GetCurrentIndex()].selectBorder.GetComponent<Image>();
        selected = fishList[PlayerPrefsData.GetCurrentIndex()].selectedButton;
        select = fishList[PlayerPrefsData.GetCurrentIndex()].selectButton;

    }
    #endregion
}
