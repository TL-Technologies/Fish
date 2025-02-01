using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EndGameBox : BaseBox
{
    private static GameObject instance;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI killAmount, highestRank, sliderText, piceAmount;
    [SerializeField] Transform fishPlayerView, fishHeadMovePoint;
    [SerializeField] Image iconItem;
    [SerializeField] GameObject unlockUI;
    [SerializeField] Button btnRetry, btnAddPiece;
    [SerializeField] private TextMeshProUGUI gameOverTxt;
    [SerializeField] private TextMeshProUGUI retryBtnText;
    [SerializeField] private Image bannerWin;
    [SerializeField] private Button homeBtn, shopBtn, settingBtn, noAdsBtn;
    
    public Fish fish;
    public static EndGameBox Setup()
    {
        if (instance == null)
        {
            // Create popup and attach it to UI

            instance = Instantiate(FindObjectOfType<GameManager>().gameEnd);
        }
        instance.SetActive(false);

        return instance.GetComponent<EndGameBox>();
    }

    protected override void Awake()
    {
        homeBtn.onClick.AddListener(OnClickHome);
        btnRetry.AddCustomListner(() =>
        {
            if (PhotonController.instance.gameType == PhotonController.GameType.SinglePlayer)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        });
        shopBtn.onClick.AddListener(OnClickShop);
        settingBtn.onClick.AddListener(OnClickSettingBtn);
        noAdsBtn.onClick.AddListener(OnClickNoAdsBtn);
    }
    
    [SerializeField] Canvas canvas;
    [SerializeField] Camera cameraUI;

    public override void Show()
    {
        base.Show();
        if (PhotonController.instance.gameType == PhotonController.GameType.SinglePlayer)
        {
            retryBtnText.text = "BATTLE";
        }
        else
        {
            retryBtnText.text = "HOME";
        }
        switch (FindObjectOfType<GameManager>().win)
        {
            case true:
                bannerWin.gameObject.SetActive(true);
                gameOverTxt.gameObject.SetActive(false);
                FindObjectOfType<GameManager>().win = false;
                break;
            case false:
                bannerWin.gameObject.SetActive(false);
                gameOverTxt.gameObject.SetActive(true);
                break;
        }

        for (int i = 0; i < FindObjectsOfType<Canvas>().Length; i++)
        {
            FindObjectsOfType<Canvas>()[i].enabled = false;
        }

        canvas.enabled = true;
        canvas.worldCamera = cameraUI;
        cameraUI.transform.parent = null;
        var s = FindObjectOfType<GameManager>().playerHighestRank;
        if (s != null)
        {
            if (s == -1)
            {
                s = Random.Range(5, 15);
            }
            else
            {
                s = FindObjectOfType<GameManager>().playerHighestRank;
            }
        }
        else
        {
            s = Random.Range(5, 15);
        }

        highestRank.text = "HIGHEST RANK : " + s;
        killAmount.text = "SCORE : " + FindObjectOfType<PlayerController>().fish.score.ToString();
        int max = 0;
        var player = FindObjectOfType<PlayerController>();
        player.gameObject.SetActive(false);
        var playerClone = Instantiate(player);
        playerClone.enabled = false;
        playerClone.GetComponent<Fish>().enabled = false;
        playerClone.transform.position = Vector3.zero;
        playerClone.transform.eulerAngles = new Vector3(0, 0, 0);
        playerClone.body.localEulerAngles = new Vector3(0, 0, 0); 
        playerClone.transform.localScale = new Vector3(3f, 3f, 3f);
        playerClone.transform.GetChild(1).gameObject.SetActive(false);
        playerClone.transform.GetComponentInChildren<Mouth>(transform).gameObject.SetActive(false);
        playerClone.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        //Camera.main.enabled = (true);
        for (int i = 0; i < FindObjectsOfType<Canvas>().Length; i++)
        {
            FindObjectsOfType<Canvas>()[i].enabled = true;
        }
    }
    public void OnClickRetry()
    {
        //GameController.isRetry = true;
        SceneManager.LoadScene(1);
    }

    private void OnClickHome()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(0);
    }

    private void OnClickShop()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(0);
        //var box = ShopBox.Setup();
        //box.Show();
    }
    
    private void OnClickSettingBtn()
    {
        SettingBox.Setup();
    }
    
    private void OnClickNoAdsBtn()
    {
        //PaymentHelper.Purchase(StringHelper.GameIAPID.ID_NO_ADS, OnNoAdsDone);
    }
    
    private void OnNoAdsDone(bool obj)
    {
        if (obj)
        {
           // GameController.instance.RemoveAds();
            noAdsBtn.gameObject.SetActive(false);
        }
       // var box = IAPBox.Setup();
        //box.Show();
    }
}
