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

    

    #endregion
}
