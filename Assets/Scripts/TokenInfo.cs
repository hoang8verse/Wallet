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

    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private GameObject _tokenDetail;
    string m_tokenAddress = "";
    string m_tokenName = "";
    string m_tokenSymbol = "";
    decimal m_tokenBalance = 0;
    // Use this for initialization
    void Start()
    {

    }
    public void SetupToken(string address, string name, decimal balance, string symbol, decimal price, float raise, decimal _balancePrice 
        , GameObject mainScreen , GameObject tokenDetail)
    {
        _mainScreen = mainScreen;
        _tokenDetail = tokenDetail;
        m_tokenAddress = address;
        m_tokenName = name;
        m_tokenSymbol = symbol;
        m_tokenBalance = balance;

        tokenName.text = name;
        tokenBalance.text = balance.ToString("N2") + " " + symbol;

        tokenPrice.text = "$ " + price.ToString("N3");
        tokenRaise.text = (raise >= 0 ? "+ " : "- ") + raise;
        balancePrice.text = "$ " + _balancePrice.ToString("N3");
    }
    public void GotoDetail()
    {
        _mainScreen.SetActive(false);
        _tokenDetail.SetActive(true);
        _tokenDetail.GetComponent<TokenDetail>().SetupToken(
            m_tokenAddress,
            m_tokenName,
            m_tokenBalance,
            m_tokenSymbol
            );
    }

}
