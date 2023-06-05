using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TokenInfo : MonoBehaviour
{
    [SerializeField] private GameObject tokenIcon;
    [SerializeField] private TextMeshProUGUI tokenName;
    [SerializeField] private TextMeshProUGUI tokenBalance;

    [SerializeField] private TextMeshProUGUI tokenPrice;
    [SerializeField] private TextMeshProUGUI tokenRaise;
    [SerializeField] private TextMeshProUGUI balancePrice;


    // Use this for initialization
    void Start()
    {

    }
    public void SetupToken(string name, decimal balance, string symbol, decimal price, float raise, decimal _balancePrice )
    {
        tokenName.text = name;
        tokenBalance.text = balance.ToString("N2") + " " + symbol;

        tokenPrice.text = "$ " + price.ToString("N3");
        tokenRaise.text = (raise >= 0 ? "+ " : "- ") + raise;
        balancePrice.text = "$ " + _balancePrice.ToString("N3");
    }
    public void GotoDetail()
    {

    }

}
