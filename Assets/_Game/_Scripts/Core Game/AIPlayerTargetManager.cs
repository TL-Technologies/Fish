using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIPlayerTargetManager : MonoBehaviour
{
    public List<Transform> targets;
    public List<Transform> spawnTargets;
    public GameObject[] aiPlayerPrefabs;
    public List<Fish> allFishes = new List<Fish>();
    private int j = 0;
    public Fish mainFish;

    private void Start()
    {
        /*int count = spawnTargets.Count / 2;
        //StartCoroutine(main());
        allFishes.Add(mainFish);
        for (int i = 0; i < count; i++)
        {
            SpawnAiPlayer();
        }*/
    }

    public void atStart()
    {
        allFishes.Clear();
        int count = spawnTargets.Count / 2;
        foreach (var s in FindObjectOfType<PlayerSpawner>().players)
        {
            allFishes.Add(s.gameObject.GetComponent<Fish>());
        }
        for (int i = 0; i < count; i++)
        {
            SpawnAiPlayer();
        }
    }
    
    public void SpawnAiPlayer()
    {
        
            GameObject randomAiPlayer = aiPlayerPrefabs[Random.Range(0, aiPlayerPrefabs.Length)];
            int randomVal = Random.Range(0, spawnTargets.Count);
            GameObject obj = Instantiate(randomAiPlayer, spawnTargets[randomVal].localPosition, Quaternion.identity);
            obj.name = obj.name + j++;
            allFishes.Add(obj.GetComponent<Fish>());
            foreach (Fish fish in allFishes)
            {
                if (fish.ismainPlayer)
                {
                    fish.fishName = FindObjectOfType<PlayerController>().fish.fishName;
                }
                else
                {
                    fish.fishName = "Player"+ Random.Range(0, 125);
                } 
            }
        
            spawnTargets.RemoveAt(randomVal);

            if(spawnTargets.Count == 0)
                ResetSpawnTargets();
        
       
       
    }

    private void ResetSpawnTargets()
    {
        spawnTargets.AddRange(GetComponentsInChildren<Transform>());
    }

    public void ResetTargets()
    {
        targets.AddRange(GetComponentsInChildren<Transform>());
    }
}