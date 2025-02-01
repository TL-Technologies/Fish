using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    [SerializeField] internal TMP_Text warningText; 
    
    
    internal void ShowWarning(string warning)
    {
        Debug.Log(warning);
        gameObject?.SetActive(true);
        gameObject?.transform.DOScaleX(1f, 0.5f).OnComplete(() =>
        {
            warningText.text = warning;
            StartCoroutine(DisableWarning());
        });
    }


    private IEnumerator DisableWarning()
    {
        yield return new WaitForSeconds(4f);
        gameObject?.transform.DOScaleX(0f, 0.3f).OnComplete(() =>
        {
            gameObject?.SetActive(false);
            warningText.text = "";
        });
        
    }
}
