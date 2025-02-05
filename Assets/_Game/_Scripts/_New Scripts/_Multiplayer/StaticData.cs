using System.Collections;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public const byte StartGame = 1;
    public const byte countDown = 2;
    
    public static IEnumerator GetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
