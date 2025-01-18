using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchingPlayerBox : MonoBehaviour
{
    private MatchingManager matchingManager;
    public Image flag;
    public Text userName;
    public GameObject loadingImg;

    private void Start()
    {
        matchingManager = FindObjectOfType<MatchingManager>();

        int randomTime = Random.Range(1, 5);

        Invoke(nameof(StartMatching), randomTime);
    }

    private void StartMatching()
    {
        int randomIndex = Random.Range(0, matchingManager.flags.Count);
        flag.sprite = matchingManager.flags[randomIndex];
        flag.gameObject.SetActive(true);
        loadingImg.SetActive(false);
        matchingManager.flags.RemoveAt(randomIndex);

        randomIndex = Random.Range(0, matchingManager.userNames.Count);
        userName.text = matchingManager.userNames[randomIndex];
        matchingManager.userNames.RemoveAt(randomIndex);

        matchingManager.Connected();
    }
}