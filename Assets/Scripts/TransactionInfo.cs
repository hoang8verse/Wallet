using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TransactionInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI transactionDate;
    [SerializeField] private GameObject transactionIcon;

    [SerializeField] private TextMeshProUGUI transactionTitle;
    [SerializeField] private TextMeshProUGUI transactionStatus;

    [SerializeField] private TextMeshProUGUI transactionBalance;

    [SerializeField] private List<Sprite> listIcons;
    string[] listStatusColor = new string[] { "#FFC400", "#00DBB6", "#00DBB6", "#E03232" }; // pending - send success - receive success - fail
    string[] listStatus = new string[] {
        "Pending",
        "Success",
        "Success",
        "Failed" }; // pending - send success - receive success - fail
    string[] listTitle = new string[] {
        "Contract Interaction",
        "Send",
        "Receive",
        "Contract Interaction" }; // pending - send success - receive success - fail
    // Use this for initialization
    void Start()
    {

    }
    public void SetupTransaction(int indexStatus, decimal balance, string symbol, string date)
    {
        transactionIcon.GetComponent<Image>().sprite = listIcons[indexStatus];
        transactionDate.text = date;
        transactionTitle.text = listTitle[indexStatus] + " " + symbol;
        transactionStatus.text = listStatus[indexStatus];
        Color textColor;
        if (ColorUtility.TryParseHtmlString(listStatusColor[indexStatus], out textColor))
        {
            transactionStatus.color = textColor;
        }
        
        transactionBalance.text = balance.ToString("N2") + " " + symbol;

    }
    public void GotoDetail()
    {

    }

}
