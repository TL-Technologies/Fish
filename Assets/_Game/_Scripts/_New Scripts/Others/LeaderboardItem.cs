using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName, txtScore;
    [SerializeField] private Image imgHighLight;
    public string name;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(string _name, string _score, bool isHighLight = false)
    {
        name = _name;
        txtName.text = _name;
        txtScore.text = _score;
        if (imgHighLight != null)
        {
            imgHighLight.gameObject.SetActive(true);
        }
        txtName.color = isHighLight ? Color.yellow : Color.white;
        txtScore.color = isHighLight ? Color.yellow : Color.white;
    }

    public void RefreshUI()
    {
        txtName.text = txtScore.text = string.Empty;
        imgHighLight.gameObject.SetActive(false);
        txtName.color = Color.white;
        txtScore.color = Color.white;
    }
}
