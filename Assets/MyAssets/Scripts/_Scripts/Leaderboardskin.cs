using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboardskin : MonoBehaviour
{
    [Header("Lvl 1")]
    public SpriteRenderer lvl_1_Body;   
    public SpriteRenderer lvl_1_tail;  
    public SpriteRenderer lvl_1_oan;   
    public SpriteRenderer lvl_1_openMouth;

    [Header("Lvl 2")]
    public SpriteRenderer lvl_2_Body;
    public SpriteRenderer lvl_2_tail;
    public SpriteRenderer lvl_2_oan;
    public SpriteRenderer lvl_2_openMouth;

    [Header("Lvl 3")]
    public SpriteRenderer lvl_3_Body;
    public SpriteRenderer lvl_3_tail;
    public SpriteRenderer lvl_3_oan;
    public SpriteRenderer lvl_3_openMouth;

    public FishSkin skins;

   
    private void Start()
    {
        skins = FindObjectOfType<FishSkin>();
        SetRandomSkin();
    }

    private void SetRandomSkin()
    {
       var randomIndex =
       FindObjectOfType<PlayerController>().fish.currentIndex;

        lvl_1_Body.sprite      =      skins.skin[randomIndex].lvl_1_Body;
        lvl_1_tail.sprite      =      skins.skin[randomIndex].lvl_1_tail;
         lvl_1_oan.sprite       =       skins.skin[randomIndex].lvl_1_oan;
   lvl_1_openMouth.sprite = skins.skin[randomIndex].lvl_1_openMouth;


        lvl_2_Body.sprite =      skins.skin[randomIndex].lvl_2_Body;
        lvl_2_tail.sprite =      skins.skin[randomIndex].lvl_2_tail;
         lvl_2_oan.sprite =       skins.skin[randomIndex].lvl_2_oan;
   lvl_2_openMouth.sprite = skins.skin[randomIndex].lvl_2_openMouth;


        lvl_3_Body.sprite =      skins.skin[randomIndex].lvl_3_Body;
        lvl_3_tail.sprite =      skins.skin[randomIndex].lvl_3_tail;
         lvl_3_oan.sprite =       skins.skin[randomIndex].lvl_3_oan;
   lvl_3_openMouth.sprite = skins.skin[randomIndex].lvl_3_openMouth;

    }
}
