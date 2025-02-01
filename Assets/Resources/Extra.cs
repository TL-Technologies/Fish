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
            
       
        GetComponent<PlayerController>().boundsCollider = GameObject.FindGameObjectWithTag("Boundry Collider").GetComponent<BoxCollider2D>();

        PlayerSpawner playerSpawner = FindObjectOfType<PlayerSpawner>();

        // Clear the list first to prevent duplicates
        playerSpawner.players.Clear();

        // Get all players and add them once
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView photonView in photonViews)
        {
            if (!playerSpawner.players.Contains(photonView)) // Prevent duplicates
            {
                playerSpawner.players.Add(photonView);
            }
        }

        // Initialize AI Target Manager
        FindObjectOfType<AIPlayerTargetManager>()?.atStart();

        if (!photonView.IsMine)
        {
            GetComponent<PlayerController>().enabled = false;
            GetComponent<Fish>().ismainPlayer = false;
        }
        else
        {
            playerSpawner.camTarget.target = transform;
        }
        }
    }
}