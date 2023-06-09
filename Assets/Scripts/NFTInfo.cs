using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NFTInfo : MonoBehaviour
{
    [SerializeField] private GameObject NFTIcon;
    [SerializeField] private TextMeshProUGUI NFTName;
    [SerializeField] private TextMeshProUGUI NFTBalance;

    [SerializeField] private TextMeshProUGUI NFTPrice;
    [SerializeField] private TextMeshProUGUI NFTRaise;
    [SerializeField] private TextMeshProUGUI balancePrice;


    // Use this for initialization
    void Start()
    {

    }
    public void SetupNFT(string name, decimal balance, string symbol)
    {
        NFTName.text = name;
        NFTBalance.text = balance + " " + symbol;
    }
    public void GotoDetail()
    {

    }

}
