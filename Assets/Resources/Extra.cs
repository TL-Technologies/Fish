using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Extra : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        GetComponent<PlayerController>().boundsCollider = GameObject.FindGameObjectWithTag("Boundry Collider").GetComponent<BoxCollider2D>();
        if (!photonView.IsMine)
        {
            GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            FindObjectOfType<PlayerSpawner>().camTarget.target = transform;
        }
    }
}
