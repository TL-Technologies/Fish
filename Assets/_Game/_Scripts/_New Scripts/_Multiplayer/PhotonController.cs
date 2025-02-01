using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static PhotonController instance;

    public enum GameType
    {
        SinglePlayer,
        MultiPlayer
    }

    #region Variables

    [Space, Header("Game Type")] 
    [SerializeField] internal GameType gameType;
    
    private Dictionary<int, GameObject> PlayerList;
    [SerializeField]private string roomCode;
    
    [Header("Player List")]
    [SerializeField] internal GameObject PlayerlistPrefab;
    [SerializeField] internal Transform PlayerlistParent;
    
    
    [Space,Header("UI Data")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button homeBtn;
    
    [Space]
    [SerializeField] private GameObject optionPage;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject joinRoomPanel;
    [SerializeField] internal GameObject multiplayerPanel;
    
    [Space]
    [SerializeField] TMP_InputField roomNameInputField;
    

    [Space, Header("Room Id")] 
    [SerializeField] private TMP_Text currentPlayers; 
    [SerializeField] private TMP_Text roomId; 
    [SerializeField] TMP_Text warning;
    
    [Space, Header("Warning Controller")] 
    [SerializeField] private WarningManager e_warningManager;
    
    [Space, Header("Allow Bots")] 
    [SerializeField] internal bool allowBots;
    
    
    
    #endregion
    
    
    #region Unity Methods

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = Random.Range(1000, 9999).ToString();
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AddCallbackTarget(this);
        createRoomButton.AddCustomListner(CreateRoom);
        joinRoomButton.AddCustomListner(OnClickJoinRoom);
        joinButton.AddCustomListner(RoomCodeEnteredAndJoin);
        homeBtn.AddCustomListner(OnClickHomeBtn);
        
    }
    

    #endregion
   

    #region Photon Callbacks
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == StaticData.StartGame)
        {
            var receivedData = (object[])photonEvent.CustomData;
            var data1 = (Player)receivedData[0];
            var data2 = (bool)receivedData[1];
            if (!PhotonNetwork.IsMasterClient)
            {
                allowBots = data2;
                PhotonNetwork.LoadLevel(2);
            }
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + message);
        StartCoroutine(ShowJoinRoomPageWarning(message));
    }
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet ");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to master ");
        UIManager.Instance.loadingScreen.SetActive(false);
    }

    public override void OnCreatedRoom()                                                           // When room gets created we get this callback
    { 
        string roomId = PhotonNetwork.CurrentRoom.Name;
        Debug.Log(roomId + " is created");
    }

    public override void OnJoinedRoom()                                                          // When other player Or I join the room Will get this
    {
        roomCode = PhotonNetwork.CurrentRoom.Name;
        currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / "+ PhotonNetwork.CurrentRoom.MaxPlayers;
        this.roomId.text = "Room Id : " + roomCode;
        optionPage.SetActive(false);
        joinRoomPanel.SetActive(false);
        roomPanel.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            UIManager.Instance.StartMultipayerGame.gameObject.SetActive(true);
            UIManager.Instance.allowBots.SetActive(true);
        }
        else
        {
            UIManager.Instance.StartMultipayerGame.gameObject.SetActive(false);
            UIManager.Instance.allowBots.SetActive(false);
        }
        if (PlayerList == null)
        {
            PlayerList = new Dictionary<int, GameObject>();

        }
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerList = Instantiate(PlayerlistPrefab, PlayerlistParent);
            playerList.transform.GetChild(0).GetComponent<Text>().text = p.NickName;
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

    public override void OnPlayerEnteredRoom(Player newPlayer)                                 // When the any other player Joins the Room all other players of room get this callback but the player who joined does not get this.
    {
        currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / "+ PhotonNetwork.CurrentRoom.MaxPlayers;
        GameObject playerList = Instantiate(PlayerlistPrefab, PlayerlistParent);
        playerList.transform.GetChild(0).GetComponent<Text>().text = newPlayer.NickName;
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

    public override void OnPlayerLeftRoom(Player otherPlayer)    // When a player lefts the rrom all other players in the room gets this
    {
        Debug.Log(otherPlayer.NickName + " is left room " + PhotonNetwork.CurrentRoom);
        e_warningManager.ShowWarning(otherPlayer.NickName + " has left room ");
        currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / "+ PhotonNetwork.CurrentRoom.MaxPlayers;
        Destroy(PlayerList[otherPlayer.ActorNumber]);
        PlayerList.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()                         // I get this if I leave the room
    {
        Debug.Log("You left room -->");
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

    private void CreateRoom()
    {
       var roomName = Random.Range(1000, 99999).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 10 });
    }
    
    private void OnClickJoinRoom()
    {
       joinRoomPanel.SetActive(true);
       optionPage.SetActive(false);
    }


    private void RoomCodeEnteredAndJoin()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrWhiteSpace(roomNameInputField.text))
        {
            StartCoroutine(ShowJoinRoomPageWarning("Please enter a valid room code"));
        }
        else
        {
            var roomCode = roomNameInputField.text;
            PhotonNetwork.JoinRoom(roomCode);
        }
    }

    private void OnClickHomeBtn()
    {
         PlayerList?.Clear();
         foreach (Transform obj in PlayerlistParent)
         {
             Destroy(obj.gameObject);
         }
        PhotonNetwork.Disconnect();
        LoadingController.Instance.mainMenu.SetActive(true);
        LoadingController.Instance.player.SetActive(true);
        optionPage.SetActive(true);
        roomPanel.SetActive(false);
        joinRoomPanel.SetActive(false);
        multiplayerPanel.SetActive(false);
        roomNameInputField.text = "";
        
    }
    
    IEnumerator ShowJoinRoomPageWarning(string msg)
    {
        warning.text = msg;
        yield return new WaitForSeconds(3);
        warning.text = "";
    }


    public void OnClickStartGame()
    {
        object[] data =
        {
            PhotonNetwork.LocalPlayer,
            allowBots
        };
        RaiseEvt(StaticData.StartGame, data, ReceiverGroup.Others);
        PhotonNetwork.LoadLevel(2);
    }

    #endregion
}
