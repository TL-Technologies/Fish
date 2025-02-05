using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiData : MonoBehaviour
{
    public static uiData Instance;
    #region Variables
    
    [Header("Player List")]
    [SerializeField] internal GameObject PlayerlistPrefab;
    [SerializeField] internal Transform PlayerlistParent;
    
    
    [Space,Header("UI Data")]
    [SerializeField] internal Button createRoomButton;
    [SerializeField] internal Button joinRoomButton;
    [SerializeField] internal Button joinButton;
    [SerializeField] internal Button homeBtn;
    
    [Space]
    [SerializeField] internal GameObject optionPage;
    [SerializeField] internal GameObject roomPanel;
    [SerializeField] internal GameObject joinRoomPanel;
    [SerializeField] internal GameObject multiplayerPanel;
    
    [Space]
    [SerializeField] internal TMP_InputField roomNameInputField;
    

    [Space, Header("Room Id")] 
    [SerializeField] internal TMP_Text currentPlayers; 
    [SerializeField] internal TMP_Text roomId; 
    [SerializeField] internal TMP_Text warning;
    
    [Space, Header("Warning Controller")] 
    [SerializeField] internal WarningManager e_warningManager;
    
    [Space, Header("Allow Bots")] 
    [SerializeField] internal bool allowBots;

    public GameObject allowBotsObject;
    
    
    
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void close()
    {
        Debug.Log("close");
        allowBotsObject.SetActive(false);
        UIManager.Instance.StartMultipayerGame.gameObject.SetActive(false);
    }
    
    public void Open()
    {
        allowBotsObject.SetActive(true);
        UIManager.Instance.StartMultipayerGame.gameObject.SetActive(true);
    }
}
