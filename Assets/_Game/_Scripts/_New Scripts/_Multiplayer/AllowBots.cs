using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowBots : MonoBehaviour
{
   public SliceOnOff sliceOnOff;

   private void OnEnable()
   {
     
      sliceOnOff.onClickOn += ToggleAllowBots;
      sliceOnOff.onClickOff += ToggleDisAllowBots;
   }

   private void OnDisable()
   {
      sliceOnOff.onClickOn -= ToggleAllowBots;
      sliceOnOff.onClickOff -= ToggleDisAllowBots;
   }

   private void Start()
   {
      PhotonController.instance.allowBots = true;
   }

   private void ToggleDisAllowBots()
   {
      PhotonController.instance.allowBots = false;
   }

   private void ToggleAllowBots()
   {
      PhotonController.instance.allowBots = true;
   }
}
