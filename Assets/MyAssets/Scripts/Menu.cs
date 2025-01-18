using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance.Play("Click");

        SceneManager.LoadScene(1);
    }

    public void RateUs()
    {
        AudioManager.Instance.Play("Click");

        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void MoreGames()
    {
        AudioManager.Instance.Play("Click");

        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
}