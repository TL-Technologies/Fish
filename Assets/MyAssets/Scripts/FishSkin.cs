using System;
using UnityEngine;

public class FishSkin : MonoBehaviour
{
    
        public string[] productIds;
        public Sprite[] katana;
    [Serializable]
    public class Skin
    {
        [Header("Lvl 1")]
        public Sprite lvl_1_Body;
        public Sprite lvl_1_tail;
        public Sprite lvl_1_oan;
        public Sprite lvl_1_openMouth;

        [Header("Lvl 2")]
        public Sprite lvl_2_Body;
        public Sprite lvl_2_tail;
        public Sprite lvl_2_oan;
        public Sprite lvl_2_openMouth;


        [Header("Lvl 3")]
        public Sprite lvl_3_Body;
        public Sprite lvl_3_tail;
        public Sprite lvl_3_oan;
        public Sprite lvl_3_openMouth;
        public bool canUse;
        public string ids;

    }

    public Skin[] skin;
}