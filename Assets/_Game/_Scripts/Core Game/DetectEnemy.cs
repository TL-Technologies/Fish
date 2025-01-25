using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DetectEnemy : MonoBehaviour
{
    public Fish fish;
    public AiPlayerController aiPlayerController;
    private bool canDetect = true;

    private void Start()
    {
        int randomTime = Random.Range(1, 5);
        //InvokeRepeating(nameof(EnableDisableDetect), randomTime, randomTime);
    }

    private void EnableDisableDetect()
    {
        canDetect =! canDetect; 
    }

    private Transform otherEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDetect && other.GetComponent<Fish>().GetScore() < fish.score)
        {
            otherEnemy = other.transform;

            Invoke(nameof(GotoEnemy), .4f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            aiPlayerController.FindTarget();
            Invoke(nameof(GotoEnemy), .4f);
        }
    }

    private void GotoEnemy()
    {
        if (otherEnemy == null)
            return;

        aiPlayerController.target = otherEnemy;
        fish.Boost();
    }
}