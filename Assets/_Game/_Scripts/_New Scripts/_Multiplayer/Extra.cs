using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Extra : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (PhotonController.instance.gameType == PhotonController.GameType.MultiPlayer)
        {
            GetComponent<PlayerController>().boundsCollider =
                GameObject.FindGameObjectWithTag("Boundry Collider").GetComponent<BoxCollider2D>();
            PlayerSpawner playerSpawner = FindObjectOfType<PlayerSpawner>();
            playerSpawner.players.Clear();
            PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
            foreach (PhotonView photonView in photonViews)
            {
                if (!playerSpawner.players.Contains(photonView)) // Prevent duplicates
                {
                    playerSpawner.players.Add(photonView);
                }
            }

            if (PhotonController.instance.allowBots)
            {
                FindObjectOfType<AIPlayerTargetManager>()?.atStart();
            }


            if (!photonView.IsMine)
            {
                Destroy(GetComponent<PlayerController>());
                GetComponent<Fish>().ismainPlayer = false;
            }
            else
            {
                playerSpawner.camTarget.target = transform;
            }
        }
    }
}