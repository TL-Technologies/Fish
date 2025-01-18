using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Fish fish;

    private void OnEnable()
    {
        fish = GetComponentInParent<Fish>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fish.GetScore() >= collision.GetComponent<Fish>().GetScore() || fish.GetScore() <= collision.GetComponent<Fish>().GetScore())
            {
                Destroy(collision.GetComponentInChildren<Mouth>());

                fish.Kill(collision.GetComponent<Fish>().score);
                collision.GetComponent<Fish>().Die();
            }
        }
    }
}
