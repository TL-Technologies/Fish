using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance;
    [Header("Username")] [SerializeField] internal TMP_InputField usernameInput;
    [SerializeField] internal Button doneButton;

    [Header("Settings")] [SerializeField] internal GameObject settingPrefab;
    [SerializeField] internal Button settingButton;

    [Header("All borders and extras For Fish")] 
    [SerializeField] internal Image border;
    [SerializeField] internal GameObject selected;
    [SerializeField] internal GameObject select;


    [Header("All borders and extras For Weapon")] 
    [SerializeField] internal Image borderWeapon;
    [SerializeField] internal GameObject selectedWeapon;
    [SerializeField] internal GameObject selectWeapon;

    [Space] [Header("Shop ")] 
    [SerializeField] internal Button ShopBtn;
    [SerializeField] internal Button shopClose;
    [SerializeField] internal GameObject shop;

    [Space]
    [SerializeField] List<FishDetailManager> fishList = new List<FishDetailManager>();
    [SerializeField] List<WeaponDetailManager> weaponList = new List<WeaponDetailManager>();

    
    [Space] [Header("Shop Internal Data ")] 
    [SerializeField] internal GameObject objShop;
    [SerializeField] internal TMP_Text fishText;
    [SerializeField] internal GameObject objWeapon;
    [SerializeField] internal TMP_Text weaponText;
    [SerializeField] internal TMP_Text name;
    [SerializeField] internal Color normalColor;
    [SerializeField] internal Color disabledColor;

    
    [Space]
    [Header("Fish Skin")]
    [SerializeField] FishSkin fishSkin;


    
    [Space]
    [Header("Start Screen Fish Data")]
    [SerializeField] internal SpriteRenderer lvl_1_Body;   
    [SerializeField] internal SpriteRenderer lvl_1_tail;  
    [SerializeField] internal SpriteRenderer lvl_1_oan;   
    [SerializeField] internal SpriteRenderer lvl_1_openMouth;


    [Space][Header("Shop scroll rects")]
    [SerializeField] internal ScrollRect fishRect;
    [SerializeField] internal ScrollRect weaponRect;
    
    [Space][Header("Play Button")]
    [SerializeField] internal Button playButton;
    [SerializeField] internal Button StartMultipayerGame;
    [SerializeField] internal GameObject allowBots;
    [SerializeField] internal Button multiplayerBtn;
    [SerializeField] internal GameObject loadingScreen;
    
    [Space][Header("Random Room")]
    [SerializeField] internal GameObject optionpage;
    [SerializeField] internal GameObject randomRoomPage;
    public bool isSingle;
    [SerializeField]internal bool isLoaded= false;

    private void Awake()
    {
        Instance = this;
    }

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

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0 && StaticData.GetRandomStatus())
        {
            ss();
        }
    }

    public void ss()
    {
        PhotonController.instance.countdownTime = 15;
        Debug.Log("GetRandomStatus");
        //StartCoroutine(ss());
        StaticData.SetRandomStatus(true);
        isSingle = true;
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connect to server");
            loadingScreen.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
        }

        StartCoroutine(Cr());
    }
    

    #region Unity Methods
    
    private void Start()
    {
        SetNameAtStart();
        SetSkin();
        FishAtStart();
        WeaponAtStart();
        doneButton.AddCustomListner(()=> {OnClickDone(); });
        settingButton.AddCustomListner(ShowSetting);
        ShopBtn.AddCustomListner(Openshop);
        shopClose.AddCustomListner(CloseShop);
        usernameInput.onEndEdit.AddListener(OnClickDone);
        playButton.AddCustomListner(OnClickPlayButton);
        StartMultipayerGame.AddCustomListner(OnClickStartGameButton);
        multiplayerBtn.AddCustomListner(OnClickMultiplayer);
    }

    private void Openshop()
    {
        shop.SetActive(true);
        fishRect.horizontalNormalizedPosition = 0f;
        weaponRect.horizontalNormalizedPosition = 0f;
    }

    private void CloseShop()
    {
        SetSkin();
        shop.SetActive(false);
    }

    #endregion


    #region Private Methods

    private void OnClickDone(string input = null)
    {
        Debug.Log(input);
        if (!string.IsNullOrEmpty(usernameInput.text) && usernameInput.text.Length >= 1 &&
            usernameInput.text.Length <= 25)
        {
            var name = usernameInput.text;
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = name;
            }
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
        int fishIndex = PlayerPrefsData.GetCurrentIndex();
        if (fishIndex < 0) fishIndex = 0; // Ensure index is not negative

        fishList[fishIndex].selectBorder.GetComponent<Image>().enabled = true;
        fishList[fishIndex].selectedButton.SetActive(true);
        fishList[fishIndex].selectButton.SetActive(false);

        border = fishList[fishIndex].selectBorder.GetComponent<Image>();
        selected = fishList[fishIndex].selectedButton;
        select = fishList[fishIndex].selectButton;
        if (PlayerPrefsData.GetCurrentIndex() == 0)
        {
            PlayerPrefsData.SetCurrentIndex(0);

        }
        else
        {
            PlayerPrefsData.SetCurrentIndex(fishIndex);
        }
    }

    void WeaponAtStart()
    {
        int weaponIndex = PlayerPrefsData.GetCurrentWeaponIndex();
        if (weaponIndex < 0) weaponIndex = 0; // Ensure index is not negative
        weaponList[weaponIndex].selectBorder.GetComponent<Image>().enabled = true;
        weaponList[weaponIndex].selectedButton.SetActive(true);
        weaponList[weaponIndex].selectButton.SetActive(false);

        borderWeapon = weaponList[weaponIndex].selectBorder.GetComponent<Image>();
        selectedWeapon = weaponList[weaponIndex].selectedButton;
        selectWeapon = weaponList[weaponIndex].selectButton;
        if (PlayerPrefsData.GetCurrentWeaponIndex() == 0)
        {
            PlayerPrefsData.SetCurrentWeaponIndex(0);
        }
        else
        {
            PlayerPrefsData.SetCurrentWeaponIndex(weaponIndex);
        }
    }

    #endregion


    public void OpenFish()
    {
        name.text = "";
        objShop.SetActive(true);
        fishText.color = normalColor;
        weaponText.color = disabledColor;
        objWeapon.SetActive(false);
        fishRect.horizontalNormalizedPosition = 0f;
        weaponRect.horizontalNormalizedPosition = 0f;
    }

    public void OpenSword()
    {
        name.text = "";
        objShop.SetActive(false);
        fishText.color = disabledColor;
        weaponText.color = normalColor;
        objWeapon.SetActive(true);
        fishRect.horizontalNormalizedPosition = 0f;
        weaponRect.horizontalNormalizedPosition = 0f;
    }


    private void SetSkin()
    {
        int randomIndex = PlayerPrefsData.GetCurrentIndex();
        if (randomIndex < 0 )
        {
            randomIndex = 0;
        }
        lvl_1_Body.sprite      =      fishSkin.skin[randomIndex].lvl_1_Body;
        lvl_1_tail.sprite      =      fishSkin.skin[randomIndex].lvl_1_tail;
        lvl_1_oan.sprite       =       fishSkin.skin[randomIndex].lvl_1_oan;
        lvl_1_openMouth.sprite = fishSkin.skin[randomIndex].lvl_1_openMouth;
        
    }


    private void OnClickPlayButton()
    {
        StaticData.SetRandomStatus(true);
        PhotonController.instance.countdownTime = 15;
        isSingle = true;
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connect to server");
            loadingScreen.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
        }

        StartCoroutine(Cr());
    }


    IEnumerator Cr()
    {
        Debug.Log("GetRandomStatus");
        var ss = PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby;
        yield return new WaitUntil(()=>ss);
        PhotonNetwork.JoinRandomRoom();
        PhotonController.instance.gameType = PhotonController.GameType.MultiPlayer;
        AudioManager.Instance.Play("Click");
    }
    
   
    
    private void OnClickStartGameButton()
    {
        AudioManager.Instance.Play("Click");
        PhotonController.instance.OnClickStartGame();
        
    }

    private void OnClickMultiplayer()
    {
        StaticData.SetRandomStatus(false);
        uiData.Instance.Open();
        isSingle = false;
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connect to server");
            loadingScreen.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
        }
        
        uiData.Instance.multiplayerPanel.SetActive(true);
        LoadingController.Instance.mainMenu.SetActive(false);
        LoadingController.Instance.player.SetActive(false);
        PhotonController.instance.gameType = PhotonController.GameType.MultiPlayer;
    }
    
    
    
}
