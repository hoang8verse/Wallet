using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.HdWallet;


public class WalletController : MonoBehaviour
{
    public static WalletController instance;
    private string currentChain = "ethereum"; // bitcoin
    private bool isMainNet = false;

    Web3 web3;
    Mnemonic mnemo;
    public string wallet_address = "";
    public string wallet_phrases = "";
    public string wallet_password = "";
    public string wallet_privateKey = "";

    public const string Words =
       "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
   

    public InputField ResultAccountAddress;
    public InputField InputWords;
    public InputField ResultPrivateKey;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //InputWords.text = Words;
    }

    public void GetHdWalletAccoutnsRequest()
    {
       GetHdWalletAccounts();
    }

    public void GetHdWalletAccounts()
    {
        var wallet = new Wallet(Words, null);
        var account = wallet.GetAccount(0);
        ResultAccountAddress.text = account.Address;
        ResultPrivateKey.text = account.PrivateKey;
        Debug.Log(account.Address);
    }
    private string CreateTwelvePhrase()
    {
        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        return mnemo.ToString();
    }

    public void OnCreateWallet()
    {
        mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        wallet_phrases = mnemo.ToString();
    }
    private void LoginByPrivateKey(string privateKey)
    {
        // var privateKey = "0x08748dd4277c872de83e184b7a3b07fd6f124007d6589515b184145228813585";
        var account = new Account(privateKey);
        web3 = new Web3(account);
    }
    private async Task GetBalance()
    {
        var balance = await web3.Eth.GetBalance.SendRequestAsync(wallet_address);
    }
    private async void SendTransaction(string toAddress, decimal value)
    {
        value = 2.11m;
        toAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
        var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(toAddress, value, 2);
    }

}
