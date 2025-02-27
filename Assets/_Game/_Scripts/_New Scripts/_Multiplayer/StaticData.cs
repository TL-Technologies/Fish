using System.Collections;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public const byte StartGame = 1;
    public const byte countDown = 2;
    public const byte StartGameRandom = 3;

    private static bool isRandomOne = false;

    public static void SetRandomStatus(bool status)
    {
        isRandomOne = status;
    }

    public static bool GetRandomStatus()
    {
        Debug.Log("GetRandomStatus --> " + isRandomOne);
        return isRandomOne;
    }
    
    public static IEnumerator GetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
