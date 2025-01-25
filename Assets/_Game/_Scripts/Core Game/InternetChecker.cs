using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetChecker : MonoBehaviour
{
    public GameObject noInternetPanel;
    public static InternetChecker Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!noInternetPanel.activeSelf)
            {
                Debug.Log("No internet connection.");
                noInternetPanel.SetActive(true);
                AudioManager.Instance.Play("WifiError");
            }
        }
        else if (noInternetPanel.activeSelf)
        {
            noInternetPanel.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
