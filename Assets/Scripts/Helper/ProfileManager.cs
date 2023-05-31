
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ProfileManager
{
    private static ProfileManager _instance = null;
    private const string ProfileKey = "WalletProfiler";
    public static void Reset()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }
    public static ProfileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProfileManager();
            }
            return _instance;
        }
    }
    private ProfileManager()
    {
    }
    string m_Network = "bsc testnet";
    string m_Address = "";
    decimal m_Balance;

    public string Network
    {
        get { return m_Network; }
        set { m_Network = value; }
    }
    public string Address
    {
        get { return m_Address; }
        set { m_Address = value; }
    }

    public decimal Balance
    {
        get { return m_Balance; }
        set { m_Balance = value; }
    }

    public void SaveProfile(string value)
    {
        Debug.Log(" SaveProfile ----- value " + value);
        PlayerPrefs.SetString(ProfileKey, value);
        PlayerPrefs.Save();
    }

    public string LoadProfile()
    {
        return PlayerPrefs.GetString(ProfileKey);
    }

    public bool CheckExistProfile()
    {
        bool isPass = false;
        string value = PlayerPrefs.GetString(ProfileKey);

        Debug.Log(" LoadProfile =========== value " + value);
        if (value != "" && value != null)
        {
            isPass = true;

        }
        return isPass;
    }
    public void ParseProfile()
    {
        string value = PlayerPrefs.GetString(ProfileKey);
        JObject data = JObject.Parse(value);

        Debug.Log(" LoadProfile data " + data);

        WalletController.instance.wallet_name = data["wallet_name"].ToString();
        WalletController.instance.wallet_phrases = data["wallet_phrases"].ToString();
        WalletController.instance.wallet_address = data["wallet_address"].ToString();
        WalletController.instance.wallet_privateKey = data["wallet_privateKey"].ToString();
        WalletController.instance.wallet_password = data["wallet_password"].ToString();
        JArray listToken = JArray.Parse(data["token_list"].ToString());
        WalletController.instance.listTokens = listToken;

    }
    public void SaveTokenList(string listToken)
    {
        string value = PlayerPrefs.GetString(ProfileKey);
        JObject jsData = JObject.Parse(value);
        jsData["token_list"] = listToken;

        PlayerPrefs.SetString(ProfileKey, Newtonsoft.Json.JsonConvert.SerializeObject(jsData));
        PlayerPrefs.Save();
    }
    public void SaveNftList(string listToken)
    {
        string value = PlayerPrefs.GetString(ProfileKey);
        JObject jsData = JObject.Parse(value);
        jsData["nft_list"] = listToken;

        PlayerPrefs.SetString(ProfileKey, Newtonsoft.Json.JsonConvert.SerializeObject(jsData));
        PlayerPrefs.Save();
    }

    public void ResetWallet()
    {
        WalletController.instance.wallet_name = "";
        WalletController.instance.wallet_phrases = "";
        WalletController.instance.wallet_address = "";
        WalletController.instance.wallet_privateKey = "";
        WalletController.instance.wallet_password = "";
        WalletController.instance.listTokens = null;

        PlayerPrefs.SetString(ProfileKey, "");
        PlayerPrefs.Save();
    }

}

