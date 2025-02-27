using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityBase.DesignPattern;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : SingletonMonoBehavier<LoadingController>
{
   [SerializeField] internal GameObject mainMenu;
   [SerializeField] internal GameObject loadingPanel;
   [SerializeField] internal GameObject player;
   [SerializeField]  Image loader;

   #region Unity Methods
   private void Start()
   {
     ShowLoading();
   }

   #endregion
   
   #region Private Methods

   private void ShowLoading()
   {
      player.SetActive(false);
      loadingPanel.SetActive(true);
      float value = 0;
      DOTween.To(() => value, x => value = x, 1, 2.5f)
         .OnUpdate(() => { loader.fillAmount = value; })
         .OnComplete(() =>
         {
            loadingPanel.SetActive(false);
            if (!StaticData.GetRandomStatus())
            {
               player.SetActive(true);
               mainMenu.SetActive(true);
            }
            loader.fillAmount = 1;
         }).SetUpdate(true);
   }

   #endregion
   
}
