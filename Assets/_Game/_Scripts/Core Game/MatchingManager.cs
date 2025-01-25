using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingManager : MonoBehaviour
{
    public List<Sprite> flags;
    public List<string> userNames;
    private int playersCount;

    public void Connected()
    {
        playersCount++;

        if (playersCount == 10)
        {
            AudioManager.Instance.Play("StartGame");
            SceneManager.LoadScene(2);
        }
    }
}
