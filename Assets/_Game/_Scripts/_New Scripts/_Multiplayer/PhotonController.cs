using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class PhotonController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static PhotonController instance;

    #region Variables

    

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
    
    #endregion


    #region Private Internal Methods

    internal void RaiseEvt(byte code, object data, ReceiverGroup options)
    {
        RaiseEventOptions receiverOptions = new RaiseEventOptions { Receivers = options };
        PhotonNetwork.RaiseEvent(code, data, receiverOptions, SendOptions.SendReliable);
    }

    #endregion
}
