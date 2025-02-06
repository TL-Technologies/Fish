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
    
    public List<RoomInfo> roomList = new List<RoomInfo>();
    
    public int countdownTime = 15;
    
    public Coroutine coroutine;

    
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
        uiData.Instance.createRoomButton.AddCustomListner(CreateRoom);
        uiData.Instance.joinRoomButton.AddCustomListner(OnClickJoinRoom);
        uiData.Instance.joinButton.AddCustomListner(RoomCodeEnteredAndJoin);
        uiData.Instance.homeBtn.AddCustomListner(OnClickHomeBtn);
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnLoaded;
    }

    private void OnLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0)
        {
            countdownTime = 15;
        }
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
        if (photonEvent.Code == StaticData.StartGameRandom)
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
        if (photonEvent.Code == StaticData.countDown)
        {
            object[] data = (object[])photonEvent.CustomData;
            countdownTime = (int)data[0];

            if (uiData.Instance.roomId != null)
                uiData.Instance.roomId.text = "Time Left: " + countdownTime;
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + message);
        if (message == "No match found")
        {
            var s = Random.Range(49999, 845999).ToString();
            PhotonNetwork.CreateRoom(s, new RoomOptions { MaxPlayers = 10, IsVisible = true, IsOpen = true });
        }
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
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefsData.GetName();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to master ");
        UIManager.Instance.loadingScreen.SetActive(false);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnCreatedRoom()                                                           // When room gets created we get this callback
    { 
        string roomId = PhotonNetwork.CurrentRoom.Name;
        Debug.Log(roomId + " is created");
    }

    public override void OnJoinedRoom()                                                          // When other player Or I join the room Will get this
    {
        roomCode = PhotonNetwork.CurrentRoom.Name;
        uiData.Instance.multiplayerPanel.SetActive(true);
        uiData.Instance.optionPage.SetActive(false);
        uiData.Instance.joinRoomPanel.SetActive(false);
        
        if (PhotonNetwork.IsMasterClient && !UIManager.Instance.isSingle)
        {
            UIManager.Instance.StartMultipayerGame.gameObject.SetActive(true);
            UIManager.Instance.allowBots.SetActive(true);
        }
        else
        {
            UIManager.Instance.StartMultipayerGame.gameObject.SetActive(false);
            UIManager.Instance.allowBots.SetActive(false);
        }
        
        if (UIManager.Instance.isSingle)
        {
            uiData.Instance.roomPanel.SetActive(true);
            LoadingController.Instance.player.SetActive(false);
            uiData.Instance.currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
            uiData.Instance.roomId.text = "";
            if (coroutine != null)
                StopCoroutine(coroutine); // Stop any previous countdown to prevent duplicate coroutines
            coroutine = StartCoroutine(StartCountdown());
        }
        else
        {
            uiData.Instance.currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
            uiData.Instance.roomId.text = "Room Id : " + roomCode;
            uiData.Instance.roomPanel.SetActive(true);
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

            if (PlayerList.ContainsKey(p.ActorNumber))
            {
                PlayerList.Remove(p.ActorNumber);
            }
            PlayerList.Add(p.ActorNumber, playerList);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        uiData.Instance.currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / "+ PhotonNetwork.CurrentRoom.MaxPlayers;
        GameObject playerList = Instantiate(PlayerlistPrefab, PlayerlistParent);
        playerList.transform.GetChild(0).GetComponent<Text>().text = newPlayer.NickName;

        if (PhotonNetwork.IsMasterClient && UIManager.Instance.isSingle) // Ensure only master client runs the countdown
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine); // Stop any existing countdown to prevent multiple coroutines
            }
            coroutine = StartCoroutine(StartCountdown());
        }

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerList.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            playerList.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (PlayerList.ContainsKey(newPlayer.ActorNumber))
        {
            PlayerList.Remove(newPlayer.ActorNumber);
        }
        PlayerList.Add(newPlayer.ActorNumber, playerList);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)    // When a player lefts the rrom all other players in the room gets this
    {
        Debug.Log(otherPlayer.NickName + " is left room " + PhotonNetwork.CurrentRoom);
        uiData.Instance.e_warningManager.ShowWarning(otherPlayer.NickName + " has left room ");
        uiData.Instance.currentPlayers.text = "Available : " + PhotonNetwork.CurrentRoom.PlayerCount + " / "+ PhotonNetwork.CurrentRoom.MaxPlayers;
        Destroy(PlayerList[otherPlayer.ActorNumber]);
        PlayerList.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()                         // I get this if I leave the room
    {
        Debug.Log("You left room -->");
        if (PlayerList?.Count > 0)
        {
            foreach (GameObject obj in PlayerList.Values)
            {
                Destroy(obj);
            }

        }
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Rooms available: " + roomList.Count);
        this.roomList = roomList;

        if (roomList.Count > 0)
        {
            Debug.Log("Rooms available: " + roomList.Count);
        }
        else
        {
            Debug.Log("No rooms available.");
            StartCoroutine(show());
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
        uiData.Instance. joinRoomPanel.SetActive(true);
        uiData.Instance.optionPage.SetActive(false);
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
        
        if (string.IsNullOrEmpty(uiData.Instance.roomNameInputField.text) || string.IsNullOrWhiteSpace(uiData.Instance.roomNameInputField.text))
        {
            StartCoroutine(ShowJoinRoomPageWarning("Please enter a valid room code"));
        }
        else
        {
            var roomCode = uiData.Instance.roomNameInputField.text;
            PhotonNetwork.JoinRoom(roomCode);
        }
    }

    private void OnClickHomeBtn()
    {
        Debug.Log("OnClickHomeBtn");
        UIManager.Instance.isSingle = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
         PlayerList?.Clear();
         foreach (Transform obj in PlayerlistParent)
         {
             Destroy(obj.gameObject);
         }
        PhotonNetwork.Disconnect();
        LoadingController.Instance.mainMenu.SetActive(true);
        LoadingController.Instance.player.SetActive(true);
        uiData.Instance.optionPage.SetActive(true);
        uiData.Instance.roomPanel.SetActive(false);
        uiData.Instance.joinRoomPanel.SetActive(false);
        uiData.Instance.multiplayerPanel.SetActive(false);
        uiData.Instance.roomNameInputField.text = "";
        
    }
    
    IEnumerator ShowJoinRoomPageWarning(string msg)
    {
        uiData.Instance.warning.text = msg;
        yield return new WaitForSeconds(3);
        uiData.Instance. warning.text = "";
    }


    public void OnClickStartGame()
    {
        if (coroutine != null )
        {
            StopCoroutine(coroutine);
        }
        object[] data =
        {
            PhotonNetwork.LocalPlayer,
            allowBots
        };
        RaiseEvt(StaticData.StartGame, data, ReceiverGroup.Others);
        PhotonNetwork.LoadLevel(2);
    }
    
    public void OnClickStartGameRandom()
    {
        if (coroutine != null )
        {
            StopCoroutine(coroutine);
        }
        object[] data =
        {
            PhotonNetwork.LocalPlayer,
            allowBots = true
        };
        RaiseEvt(StaticData.StartGame, data, ReceiverGroup.Others);
        if (UIManager.Instance.isSingle)
        {
            allowBots = true;
        }
        PhotonNetwork.LoadLevel(2);
    }
    
    private IEnumerator StartCountdown()
    {
        while (countdownTime >= 0 && UIManager.Instance.isSingle)
        {
            if (PhotonNetwork.IsMasterClient) // Ensure only master client controls the countdown
            {
                Debug.Log("Time Left: " + countdownTime);

                if (uiData.Instance.roomId != null)
                    uiData.Instance.roomId.text = "Time Left: " + countdownTime;

                object[] data = { countdownTime };
                RaiseEvt(StaticData.countDown, data, ReceiverGroup.Others); // Send event to sync countdown
            }
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        Debug.Log("Countdown Complete!");
        if (UIManager.Instance.isSingle && SceneManager.GetActiveScene().buildIndex == 0)
        {
            OnClickStartGameRandom();  
        }
        
    }

    IEnumerator show()
    {
        if (UIManager.Instance.isSingle)
        {
            uiData.Instance.warningText.text = "No Rooms Avilable. Please click on PLAY again to join";
            uiData.Instance.warningText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            uiData.Instance.warningText.gameObject.SetActive(false);
        }
    }

    #endregion
}
