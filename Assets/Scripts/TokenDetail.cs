using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class TokenDetail : MonoBehaviour
{
    [SerializeField] private GameObject tokenIcon;
    [SerializeField] private TextMeshProUGUI tokenName;
    [SerializeField] private TextMeshProUGUI tokenBalance;

    [SerializeField] private GameObject transactionPrefab;
    [SerializeField] ScrollRect transactionList;
    string tokenAddress = "";
    string tokenSymbol = "";
    JArray jtransactionList = new JArray();
    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartSetup());
    }
    public void SetupToken(string address, string name, decimal balance, string symbol)
    {
        tokenAddress = address;
        tokenSymbol = symbol;

        tokenName.text = name;
        tokenBalance.text = balance + " " + symbol;
        
    }

    private IEnumerator StartSetup()
    {
        yield return new WaitForSeconds(0.2f);
        LoadTransactionList();
    }

    public async void LoadTransactionList()
    {

        Transform contentTransform = transactionList.content;

        // Iterate through all child objects
        if (contentTransform.childCount > 0)
        {
            for (int i = contentTransform.childCount - 1; i >= 0; i--)
            {
                // Destroy each child object
                Destroy(contentTransform.GetChild(i).gameObject);
            }
        }

        Task<JArray> jArrayTask = WalletController.instance.GetTransactionHistory("0x8301F2213c0eeD49a7E28Ae4c3e91722919B8B47");
        JArray jArray = await jArrayTask;
        jtransactionList = jArray;

        for (int i = 0; i < jtransactionList.Count; i++)
        {
            GameObject instance = Instantiate(transactionPrefab, transactionList.content);

            string _status = jtransactionList[i]["Status"].ToString();
            int indexStatus = ParseStatus(_status);
            instance.GetComponent<TransactionInfo>().SetupTransaction(
                indexStatus,
                (decimal)jtransactionList[i]["Value"],
                tokenSymbol,
                jtransactionList[i]["Date"].ToString()
                );
            //instance.name = listTokens[i]["symbol"].ToString();

        }
    }
    int ParseStatus(string status)
    {
        int indexStatus = 0;
        switch (status)
        {
            case "Pending":
                indexStatus = 0;
                break;
            case "Sender":
                indexStatus = 1;
                break;
            case "Receiver":
                indexStatus = 2;
                break;
            case "Failed":
                indexStatus = 3;
                break;
            default:
                indexStatus = 0;
                break;
        }
        return indexStatus;
    }
    public void GotoDetail()
    {

    }
    public void SendTransaction()
    {

    }
    public void ReceiveTransaction()
    {

    }
}
