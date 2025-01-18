using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
            SceneManager.LoadScene(1);
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
        killAmount.text = "SCORE : " +FindObjectOfType<PlayerController>().fish.score.ToString();
      //  var dataUnlock = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex);
        int max = 0;


        /*if (dataUnlock == null)
        {
            unlockUI.gameObject.SetActive(false);
            btnRetry.gameObject.SetActive(true);
        }
        else
        {
            max = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex).unlockCondision;
            if (dataUnlock.isFish)
            {
                var fish = GameController.instance.fishDatabase.GetFishOObjectData(dataUnlock.fishType);
                iconItem.sprite = fish.icon;
                piceAmount.text = "+ " + dataUnlock.
                    fishPieceReward.ToString();
                DataManager.currentPieceReward = dataUnlock.fishPieceReward;
                slider.value = ((float)DataManager.unlockProgressAmount / (float)max);
                sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";
            }
            else
            {
                var weapon = GameController.instance.weaponDatabase.GetWeaponObjectData(dataUnlock.weaponType);
                iconItem.sprite = weapon.icon;
                piceAmount.text = "+ " + dataUnlock.fishPieceReward.ToString();
                DataManager.currentPieceReward = dataUnlock.fishPieceReward;
                slider.value = ((float)DataManager.unlockProgressAmount / (float)max);
                sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";
            }
        }*/

        
        var player = FindObjectOfType<PlayerController>();
        player.gameObject.SetActive(false);
        var playerClone = Instantiate(player);
        playerClone.enabled = false;
        playerClone.GetComponent<Fish>().enabled = false;
        playerClone.transform.position = Vector3.zero;
        playerClone.transform.eulerAngles = new Vector3(0, 0, 0);
        playerClone.body.localEulerAngles = new Vector3(0, 0, 0);
        Debug.Log("Rot 1" +playerClone.transform.rotation);
        Debug.Log("Rot 2" +playerClone.transform.localRotation);
        playerClone.transform.localScale = new Vector3(3f, 3f, 3f);  
        playerClone.transform.GetChild(1).gameObject.SetActive(false);
        playerClone.transform.GetComponentInChildren<Mouth>().enabled = false;
        playerClone.gameObject.SetActive(true);
        Debug.Log("Added");
        //playerClone.gameObject.layer = LayerMask.NameToLayer("UI");
        
        //playerClone.fish..gameObject.SetActive(true);
        /*if (player)
        {
            var playerClone = Instantiate(player, fishPlayerView);
            playerClone.enabled = false;
            playerClone.transform.localPosition = Vector3.zero;
            playerClone.Skin.gameObject.SetActive(true);

            Transform[] chids = playerClone.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < chids.Length; i++)
            {
                chids[i].gameObject.layer = LayerMask.NameToLayer("UI");
            }
            playerClone.Skin.transform.parent.rotation = Quaternion.LookRotation(Vector3.right * -90);
            var imageBorn = playerClone.GetComponentInChildren<ImageBornFish>();
            if (imageBorn != null) Destroy(imageBorn.gameObject);

            fishHeads = playerClone.GetComponentsInChildren<FishHead>(true);

            slider.value = ((float)DataManager.unlockProgressAmount / (float)max);
            sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";
            killAmount.text = $"Total Kills: {playerClone.killAmount}";
            highestRank.text = $"HIGHEST RANK: {UIManager.Instance.playerHighestRank}";

            Observable.Timer(TimeSpan.FromSeconds(1f), Scheduler.MainThreadIgnoreTimeScale).Subscribe(_ =>
            {
                slider.value = ((float)DataManager.unlockProgressAmount / (float)max);
                sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";
                killAmount.text = $"Total Kills: {playerClone.killAmount}";
                if(playerClone.killAmount <= 0) btnRetry.gameObject.SetActive(true);
                for (int i = 0; i < fishHeads.Length; i++)
                {
                    int index = i;
                    fishHeads[i].gameObject.SetActive(true);
                    fishHeads[i].transform.parent = null;
                    fishHeads[i].MoveToPoint(fishHeadMovePoint.position, () =>
                    {
                        if (index >= fishHeads.Length - 1) btnRetry.gameObject.SetActive(true);
                        DataManager.unlockProgressAmount++;

                        slider.value = ((float)DataManager.unlockProgressAmount / max);
                        sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

                        if (DataManager.unlockProgressAmount >= max)
                        {
                            DataManager.CountUnlockBoxShow++;
                            Debug.Log(DataManager.CountUnlockBoxShow);
                            var box = UnlockBox.Setup();
                            box.Show();
                            // if (dataUnlock.isFish)
                            // {
                            //     GameController.instance.unlockData.fishCanUnlocked.Add(GameController.instance.fishDatabase.GetFishOObjectData(dataUnlock.fishType));
                            //     DataManager.StringItemUnLock();
                            // }
                            // else
                            // {
                            //     GameController.instance.unlockData.weaponCanUnlocked.Add(GameController.instance.weaponDatabase.GetWeaponObjectData(dataUnlock.weaponType));
                            //     DataManager.StringItemUnLock();
                            // }
                            DataManager.unlockIndex++;
                            Debug.Log(DataManager.unlockIndex);
                            DataManager.unlockProgressAmount = 0;
                            max = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex).unlockCondision;
                            var dataNewUnlock = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex);
                            if (dataNewUnlock == null)
                            {
                                unlockUI.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (dataNewUnlock.isFish)
                                {

                                    var fish = GameController.instance.fishDatabase.GetFishOObjectData(dataNewUnlock.fishType);
                                    iconItem.sprite = fish.icon;
                                    piceAmount.text = "+ " + dataNewUnlock.fishPieceReward.ToString();

                                    slider.value = ((float)DataManager.unlockProgressAmount / max);
                                    sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

                                    DataManager.currentPieceReward = dataNewUnlock.fishPieceReward;
                                }
                                else
                                {

                                    var weapon = GameController.instance.weaponDatabase.GetWeaponObjectData(dataNewUnlock.weaponType);
                                    iconItem.sprite = weapon.icon;
                                    piceAmount.text = "+ " + dataNewUnlock.fishPieceReward.ToString();

                                    slider.value = ((float)DataManager.unlockProgressAmount / max);
                                    sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

                                    DataManager.currentPieceReward = dataNewUnlock.fishPieceReward;
                                }
                            }
                        }
                    }, (i + 1) * 0.25f);
                }
            }).AddTo(this);
        }*/
    }
    /*public void AddPieceWithAds()
    {
        if (DataManager.unlockIndex > GameController.instance.unlockDatabase.unlockDataObjects.Count)
        {
            unlockUI.gameObject.SetActive(false);
            btnAddPiece.gameObject.SetActive(false);
            return;
        }
        Debug.Log("Show Video Reward");
        Action actionSuccess = () =>
        {
            DataManager.LastTimeShowAdsAddPiece = DateTime.Now;
            if (RemoteConfigControl.instance.AddPieceIsOn < 1 || DataManager.LastTimeShowAdsAddPiece.AddSeconds(RemoteConfigControl.instance.AddPiece_Cooldown) > DateTime.Now)
            {
                btnAddPiece.gameObject.SetActive(false);
            }
            
            DataManager.unlockProgressAmount += DataManager.currentPieceReward;
            var dataUnlock = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex);
            int max = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex).unlockCondision;
            slider.value = ((float)DataManager.unlockProgressAmount / max);
            sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

            if (DataManager.unlockProgressAmount >= max)
            {
                int residualValue = DataManager.unlockProgressAmount - max;
                Debug.Log(residualValue);
                if (dataUnlock.isFish)
                {
                    GameController.instance.unlockData.fishCanUnlocked.Add(GameController.instance.fishDatabase.GetFishOObjectData(dataUnlock.fishType));
                    DataManager.StringItemUnLock();
                }
                else
                {
                    GameController.instance.unlockData.weaponCanUnlocked.Add(GameController.instance.weaponDatabase.GetWeaponObjectData(dataUnlock.weaponType));
                    DataManager.StringItemUnLock();
                }
                DataManager.unlockIndex++;
                DataManager.unlockProgressAmount = residualValue;
                max = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex).unlockCondision;
                var dataNewUnlock = GameController.instance.unlockDatabase.GetUnlockDataObject(DataManager.unlockIndex);
                
                if (dataNewUnlock == null)
                {
                    unlockUI.gameObject.SetActive(false);
                    btnAddPiece.gameObject.SetActive(false);
                }
                else
                {
                    if (dataNewUnlock.isFish)
                    {

                        var fish = GameController.instance.fishDatabase.GetFishOObjectData(dataNewUnlock.fishType);
                        iconItem.sprite = fish.icon;
                        piceAmount.text = "+ " + dataNewUnlock.fishPieceReward.ToString();

                        slider.value = ((float)DataManager.unlockProgressAmount / max);
                        sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

                        DataManager.currentPieceReward = dataNewUnlock.fishPieceReward;
                    }
                    else
                    {

                        var weapon = GameController.instance.weaponDatabase.GetWeaponObjectData(dataNewUnlock.weaponType);
                        iconItem.sprite = weapon.icon;
                        piceAmount.text = "+ " + dataNewUnlock.fishPieceReward.ToString();

                        slider.value = ((float)DataManager.unlockProgressAmount / max);
                        sliderText.text = $"{DataManager.unlockProgressAmount} / {max}";

                        DataManager.currentPieceReward = dataNewUnlock.fishPieceReward;
                    }
                }
            }
        };

        /*if (RemoteConfigControl.instance.AddPieceIsOn >= 1 && DataManager.LastTimeShowAdsAddPiece.AddSeconds(RemoteConfigControl.instance.AddPiece_Cooldown) < DateTime.Now)
            AdsManager.ins.ShowRewardBasedVideo();#1#
    }*/
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
        SceneManager.LoadScene(0);
    }

    private void OnClickShop()
    {
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
