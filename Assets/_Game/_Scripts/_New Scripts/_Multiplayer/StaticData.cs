using System.Collections;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    
    
    public static IEnumerator GetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
