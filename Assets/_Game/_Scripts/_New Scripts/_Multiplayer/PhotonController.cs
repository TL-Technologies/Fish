using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static PhotonController instance;

    #region Variables

    private Dictionary<int, GameObject> PlayerList;
    
    [Header("Player List")]
    [SerializeField] internal GameObject PlayerlistPrefab;
    [SerializeField] internal Transform PlayerlistParent;

    #endregion
    
    
    #region Unity Methods

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AddCallbackTarget(this);
    }

    #endregion
   

    #region Photon Callbacks
    public void OnEvent(EventData photonEvent)
    {
       
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + message);
    }
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet ");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to master ");
    }

    public override void OnCreatedRoom()                                                           // Getting Room id
    {
        Debug.Log(PhotonNetwork.CurrentRoom + " is created!");
        string roomId = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Room id " + roomId);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("On room Joined");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Joined");
        if (PlayerList == null)
        {
            PlayerList = new Dictionary<int, GameObject>();

        }
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerList = Instantiate(PlayerlistPrefab, PlayerlistParent);
            playerList.transform.GetChild(0).GetComponent<TMP_Text>().text = p.NickName;
            if(p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerList.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                playerList.transform.GetChild(1).gameObject.SetActive(false );
            }
            PlayerList.Add(p.ActorNumber, playerList);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerList = Instantiate(PlayerlistPrefab, PlayerlistParent);
        playerList.transform.GetChild(0).GetComponent<TMP_Text>().text = newPlayer.NickName;
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerList.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            playerList.transform.GetChild(1).gameObject.SetActive(false);
        }
        PlayerList.Add(newPlayer.ActorNumber, playerList);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(PlayerList[otherPlayer.ActorNumber]);
        PlayerList.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
        if (PlayerList.Count > 0)
        {
            foreach (GameObject obj in PlayerList.Values)
            {
                Destroy(obj);
            }

        }
    }
    
    
    #endregion


    #region Private Internal Methods

    internal void RaiseEvt(byte code, object data, ReceiverGroup options)
    {
        RaiseEventOptions receiverOptions = new RaiseEventOptions { Receivers = options };
        PhotonNetwork.RaiseEvent(code, data, receiverOptions, SendOptions.SendReliable);
    }

    #endregion
}
