using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] internal  GameObject PlayerPrefab;
    [SerializeField] internal  GameManager manager;
    [SerializeField] internal  CameraFollow camTarget;
    public List<PhotonView> players;
    
    
    void Start()
    {
        if (PhotonController.instance.gameType == PhotonController.GameType.SinglePlayer)
        {
           var player = Instantiate(PlayerPrefab);
           manager.playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            var s = FindObjectOfType<AIPlayerTargetManager>();
            int randomVal = Random.Range(0, s.spawnTargets.Count);
            var player = PhotonNetwork.Instantiate(PlayerPrefab.name, s.spawnTargets[randomVal].localPosition, Quaternion.identity);
            Debug.Log(player.name);
            player.GetComponent<PlayerController>().boundsCollider = GameObject.FindGameObjectWithTag("Boundry Collider").GetComponent<BoxCollider2D>();
            if (player.GetPhotonView().IsMine)
            {
                manager.playerController = player.GetComponent<PlayerController>();
               //camTarget.target = player.transform;
                //player.transform.position = Vector3.zero;
                manager.playerController.joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
            }
           

        }
    }
}
