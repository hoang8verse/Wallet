﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NBitcoin;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.HdWallet;
using Nethereum.Signer;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.JsonRpc.Client;


public class WalletController : MonoBehaviour
{
    public static WalletController instance;
    public string currentNetwork = "bsc testnet";

    Wallet wallet;
    Web3 web3;
    Mnemonic mnemo;
    public string wallet_name = "";
    public string wallet_address = "";
    public string wallet_phrases = "";
    public string wallet_password = "";
    public string wallet_privateKey = "";
    public JArray listTokens;
    public JArray listNFTs;

    //public const string Words =
    //   "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";


    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        listTokens = new JArray();
        listNFTs = new JArray();
        //CheckNFTToken();
    }

    public bool VerifySeedPhrase(string seedPhrase)
    {
        bool isValid = false;
        try
        {
            // Generate a wallet from the seed phrase
            Wallet wallet = new Wallet(seedPhrase, string.Empty);

            // Check if the generated wallet address is valid
            isValid = AddressUtil.Current.IsValidEthereumAddressHexFormat(wallet.GetAccount(0).Address);
        }
        catch
        {
            // An exception occurred, indicating an invalid seed phrase
            isValid = false;
        }

        return isValid;
    }
    public bool VerifyWalletByPrivateKey(string privateKey)
    {
        bool isValid = false;
        try
        {

            // Get the public address from the private key
            string address = Nethereum.Signer.EthECKey.GetPublicAddress(privateKey);

            // Check if the generated address is valid
            isValid = AddressUtil.Current.IsValidEthereumAddressHexFormat(address);
        }
        catch
        {
            // An exception occurred, indicating an invalid private key
            isValid = false;
        }

        return isValid;
    }
    public void SetupWallet()
    {
        if(wallet_phrases != "")
        {
            wallet = new Wallet(wallet_phrases, string.Empty);
            // Get the wallet address
            wallet_address = wallet
                .GetAccount(0) // Assuming you want the first account's address
                .Address;

            wallet_privateKey = wallet.GetAccount(0).PrivateKey;
        }
        else if (wallet_privateKey != "")
        {
            wallet_address = EthECKey.GetPublicAddress(wallet_privateKey);
        }
                
        Debug.Log("wallet_address: " + wallet_address);
        Debug.Log("wallet_privateKey: " + wallet_privateKey);

        JObject jsData = new JObject();
        jsData.Add("wallet_name", wallet_name);
        jsData.Add("wallet_phrases", wallet_phrases);
        jsData.Add("wallet_address", wallet_address);
        jsData.Add("wallet_privateKey", wallet_privateKey);
        jsData.Add("wallet_password", wallet_password);
        jsData.Add("token_list", "");
        jsData.Add("nft_list", "");

        ProfileManager.Instance.SaveProfile(Newtonsoft.Json.JsonConvert.SerializeObject(jsData));

        AddCustomTokenByAddress(GetMainToken(currentNetwork));

    }
    public string GetWalletAddressBySeedPhrase(string seedPhrase)
    {
        wallet = new Wallet(seedPhrase, string.Empty);
        // Get the wallet address
        string address = wallet
            .GetAccount(0) // Assuming you want the first account's address
            .Address;
        Debug.Log("Wallet Address: " + address);
        return address;
    }
    public string GetWalletAddressByPrivate(string privateKey)
    {
        string address = EthECKey.GetPublicAddress(privateKey);

        // Print the wallet address
        Debug.Log("Wallet Address: " + address);
        return address;
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
    private async Task GetBalanceEth()
    {
        var balance = await web3.Eth.GetBalance.SendRequestAsync(wallet_address);
    }
    private async Task<JObject> GetTokenABI(string abiApiUrl)
    {
        // Try to load the ABI from a local file first
        //if (!string.IsNullOrEmpty(abiFilePath) && File.Exists(abiFilePath))
        //{
        //    return await File.ReadAllTextAsync(abiFilePath);
        //}

        // If the local file is not found, make an API request to retrieve the ABI
        if (!string.IsNullOrEmpty(abiApiUrl))
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(abiApiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Adjust the parsing logic based on the API response format
                    dynamic apiResponse = JsonConvert.DeserializeObject(responseBody);

                    string tokenAbi = apiResponse["result"];

                    Debug.Log("apiResponse apiResponse: " + apiResponse);
                    return apiResponse;
                }
                catch (HttpRequestException ex)
                {
                    Debug.LogError("Failed to retrieve ABI from API: " + ex.Message);
                }
            }
        }

        return null;
    }

    string GetApiUrlContractABI(string network, string contractAddress)
    {
        string _url = "";
        switch (network)
        {
            case "bsc testnet":
                _url = $"https://api-testnet.bscscan.com/api?module=contract&action=getabi&address={contractAddress}";
                break;
            case "bsc":
                _url = $"https://api.bscscan.com/api?module=contract&action=getabi&address={contractAddress}";
                break;
            case "polygon testnet":
                _url = $"https://api-testnet.polygonscan.com/api?module=contract&action=getabi&address={contractAddress}";
                break;
            case "polygon":
                _url = $"https://api.polygonscan.com/api?module=contract&action=getabi&address={contractAddress}";
                break;
            case "avalanche testnet":
                // Replace [Network Name] with the name of the Avalanche Testnet network (e.g., fuji)
                string networkName = "fuji";
                _url = $"https://{networkName}-api.avax-test.network/v2/contract/{contractAddress}/abi";
                break;
            case "avalanche":
                _url = $"https://explorerapi.avax.network/v2/contract/{contractAddress}/abi";
                break;
            default:
                // default bsc testnet
                _url = $"https://api-testnet.bscscan.com/api?module=contract&action=getabi&address={contractAddress}";
                break;
        }
        return _url;
    }
    string GetRpcUrl(string network)
    {
        string _url = "";
        switch (network)
        {
            case "bsc testnet":
                _url = "https://data-seed-prebsc-1-s1.binance.org:8545/";
                break;
            case "bsc":
                _url = "https://bsc-dataseed.binance.org/";
                break;
            case "polygon testnet":
                _url = "https://rpc-mumbai.maticvigil.com/";
                break;
            case "polygon":
                _url = "https://polygon-rpc.com/";
                break;
            case "avalanche testnet":
                _url = "https://api.avax-test.network/ext/bc/C/rpc";
                break;
            case "avalanche":
                _url = "https://api.avax.network/ext/bc/C/rpc";
                break;
            default:
                // default bsc testnet
                _url = "https://data-seed-prebsc-1-s1.binance.org:8545/";
                break;
        }
        return _url;
    }
    string GetMainToken(string network)
    {
        string _mainTokenAddress = "";
        switch (network)
        {
            case "bsc testnet":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            case "bsc":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            case "polygon testnet":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            case "polygon":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            case "avalanche testnet":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            case "avalanche":
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
            default:
                // default bsc testnet
                _mainTokenAddress = "0xae13d989daC2f0dEbFf460aC112a837C89BAa7cd";
                break;
        }
        return _mainTokenAddress;
    }
    
    public async Task<JObject> TokenInformation(string walletAddress, string tokenContractAddress)
    {
        // Set up the RPC client
        string rpcUrl = GetRpcUrl(currentNetwork);
        var client = new RpcClient(new Uri(rpcUrl));
        var web3 = new Web3(client);

        // Load the token contract using the specific ABI
        string abiApiUrl = GetApiUrlContractABI(currentNetwork, tokenContractAddress);
        Debug.Log("GetTokenBalance abiApiUrl: " + abiApiUrl);
        JObject jTokenAbi = await GetTokenABI(abiApiUrl);
        string contractABI = jTokenAbi["result"].ToString();

        var tokenContract = web3.Eth.GetContract(contractABI, tokenContractAddress);
        // Functions to retrieve token information
        Function balanceOfFunction = tokenContract.GetFunction("balanceOf");
        Function symbolFunction = tokenContract.GetFunction("symbol");
        Function nameFunction = tokenContract.GetFunction("name");
        Function decimalsFunction = tokenContract.GetFunction("decimals");
        Function totalSupplyFunction = tokenContract.GetFunction("totalSupply");

        // Call the functions to get token information
        var balance = await balanceOfFunction.CallAsync<BigInteger>(walletAddress);
        // Convert the balance to decimal format
        decimal balanceDecimal = Web3.Convert.FromWei(balance);

        var symbol = await symbolFunction.CallAsync<string>();
        var name = await nameFunction.CallAsync<string>();
        var decimals = await decimalsFunction.CallAsync<uint>();
        var totalSupply = await totalSupplyFunction.CallAsync<BigInteger>();

        // Display the token information
        Debug.Log($"Symbol: {symbol}");
        Debug.Log($"Name: {name}");
        Debug.Log($"Decimals: {decimals}");
        Debug.Log($"Total Supply: {totalSupply}");

        JObject jToken = new JObject();
        jToken.Add("address", tokenContractAddress);
        jToken.Add("name", name);
        jToken.Add("symbol", symbol);
        jToken.Add("decimals", decimals);
        jToken.Add("balance", balanceDecimal.ToString());
        return jToken;

    }
    public async Task<JObject> NFTInformation(string walletAddress, string nftContractAddress)
    {
        // Set up the RPC client
        string rpcUrl = GetRpcUrl(currentNetwork);
        var client = new RpcClient(new Uri(rpcUrl));
        var web3 = new Web3(client);

        // Load the token contract using the specific ABI
        string abiApiUrl = GetApiUrlContractABI(currentNetwork, nftContractAddress);
        Debug.Log("GetTokenBalance abiApiUrl: " + abiApiUrl);
        JObject jTokenAbi = await GetTokenABI(abiApiUrl);
        string contractABI = jTokenAbi["result"].ToString();

        var tokenContract = web3.Eth.GetContract(contractABI, nftContractAddress);
        // Functions to retrieve token information
        Function balanceOfFunction = tokenContract.GetFunction("balanceOf");
        Function symbolFunction = tokenContract.GetFunction("symbol");
        Function nameFunction = tokenContract.GetFunction("name");
        Function tokenURIFunction = tokenContract.GetFunction("tokenURI");
        Function tokenIdFunction = tokenContract.GetFunction("tokenByIndex");
        Function totalSupplyFunction = tokenContract.GetFunction("totalSupply");


        // Call the functions to get token information
        var balance = await balanceOfFunction.CallAsync<BigInteger>(walletAddress);
        string tokenIDs = "";
        for (BigInteger i = 0; i < balance; i++)
        {
            var tokenIDFunction = tokenContract.GetFunction("tokenOfOwnerByIndex");
            BigInteger tokenID = await tokenIDFunction.CallAsync<BigInteger>(walletAddress, i);
            //Debug.Log($"tokenIdd i : { i } : {tokenID}");
            tokenIDs += tokenID.ToString() + ((i < balance - 1) ? "," : "");
        }
        //var tokenIDFunction = tokenContract.GetFunction("tokenOfOwnerByIndex");
        //BigInteger tokenID = await tokenIDFunction.CallAsync<BigInteger>(walletAddress, 0);

        var symbol = await symbolFunction.CallAsync<string>();
        var name = await nameFunction.CallAsync<string>();
        //var tokenIds = await tokenIdFunction.CallAsync<uint256>();
        //var metadataTokenURI = await tokenURIFunction.CallAsync<string>(tokenID);
        var totalSupply = await totalSupplyFunction.CallAsync<BigInteger>();

        // Display the token information
        Debug.Log($"Symbol: {symbol}");
        Debug.Log($"Name: {name}");
        Debug.Log($"tokenIds: {tokenIDs}");
        //Debug.Log($"tokenURI: {metadataTokenURI}");
        Debug.Log($"Total Supply: {totalSupply}");
        Debug.Log($"balance: {balance}");

        JObject jToken = new JObject();
        jToken.Add("address", nftContractAddress);
        jToken.Add("name", name);
        jToken.Add("symbol", symbol);
        jToken.Add("tokenIds", tokenIDs);
        jToken.Add("balance", balance.ToString());
        return jToken;

    }

    public async void AddCustomTokenByAddress(string tokenAddress)
    {
        Task<JObject> tokenTask = TokenInformation(
            wallet_address,
            tokenAddress
            );
        JObject token = await tokenTask;

        listTokens.Add(token);
        //Debug.Log("listTokens === " + listTokens[0].ToString());
        ProfileManager.Instance.SaveTokenList(listTokens.ToString());

    }
    public void AddCustomTokenByObject(JObject token)
    {
        listTokens.Add(token);
        //Debug.Log("listTokens === " + listTokens[0].ToString());
        ProfileManager.Instance.SaveTokenList(listTokens.ToString());
    }
    public async void AddCustomNFTByAddress(string tokenAddress)
    {
        Task<JObject> tokenTask = NFTInformation(
            wallet_address,
            tokenAddress
            );
        JObject token = await tokenTask;

        listNFTs.Add(token);
        //Debug.Log("listNFTs === " + listNFTs[0].ToString());
        ProfileManager.Instance.SaveNftList(listNFTs.ToString());

    }
    public void AddCustomNFTByObject(JObject token)
    {
        listNFTs.Add(token);
        ProfileManager.Instance.SaveNftList(listNFTs.ToString());

    }
    public async Task<bool> CheckValidToken(string contractAddress)
    {

        string abiApiUrl = GetApiUrlContractABI(currentNetwork, contractAddress);
        Debug.Log("GetTokenBalance abiApiUrl: " + abiApiUrl);
        JObject jTokenAbi = await GetTokenABI(abiApiUrl);
        Debug.Log("jTokenAbi === " + jTokenAbi);

        string status = jTokenAbi["status"].ToString();
        if(status == "1")
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool CheckExistTokenAdded(string contractAddress)
    {
        bool isExist = false;
        for (int i = 0; i < listTokens.Count; i++)
        {
            if(contractAddress == listTokens[i]["address"].ToString())
            {
                isExist = true;
            }
        }
        return isExist;
    }
    async void CheckNFTToken()
    {
        string tokenAddress = "0x8E1096F8843a6AA42Da12Fe657BB482773E44C01";
        Task<bool> tokenAbiTask = CheckValidToken(tokenAddress);
        bool isValidToken = await tokenAbiTask;

      Task<JObject> tokenTask = NFTInformation(
            "0xd675524331cD55c5145E04Ff1E9C7D88684766b3",
            tokenAddress
            );
        JObject token = await tokenTask;
    }

    private async void SendTransaction(string toAddress, decimal value)
    {
        value = 2.11m;
        toAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
        var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(toAddress, value, 2);
    }

}
