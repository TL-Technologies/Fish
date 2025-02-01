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
           player.GetComponent<PlayerController>().boundsCollider = GameObject.FindGameObjectWithTag("Boundry Collider").GetComponent<BoxCollider2D>();
           manager.playerController.joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
           camTarget.target = player.transform;
           FindObjectOfType<AIPlayerTargetManager>().mainFish = player.GetComponent<Fish>();
           FindObjectOfType<AIPlayerTargetManager>()?.atStart();
           FindObjectOfType<SpeedBoostButton>().playerFish = player.GetComponent<Fish>();
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
                FindObjectOfType<SpeedBoostButton>().playerFish = player.GetComponent<Fish>();
               //camTarget.target = player.transform;
               float offsetX = Random.Range(-50f, 0f);
               float offsetY = 0f; // Keep Y fixed if it's on the ground
               float offsetZ = Random.Range(-50f, 50f);

               player.transform.position = new Vector3(offsetX, offsetY, offsetZ);
                manager.playerController.joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
            }
           

        }
    }
}
