using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class AddCustomToken : MonoBehaviour
{
    [SerializeField] private GameObject mainScreenObject;
    [SerializeField] private Button btnCancel;

    // network
    [SerializeField] private TMP_InputField inputNetwork;
    [SerializeField] private List<Sprite> iconNetwork;

    // address
    [SerializeField] private TMP_InputField inputAddress;
    [SerializeField] private GameObject avaliableAddress;
    [SerializeField] private GameObject invaildAddress;
    [SerializeField] private Button btnPaste;
    [SerializeField] private Button btnScan;

    // name
    [SerializeField] private TMP_InputField inputName;
    // symbol
    [SerializeField] private TMP_InputField inputSymbol;
    // decimals
    [SerializeField] private TMP_InputField inputDecimals;

    [SerializeField] private GameObject btnAdd;
    [SerializeField] private List<Sprite> buttonAdds;

    string m_WalletAddress = "";
    JObject m_token;
    // Use this for initialization
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        inputNetwork.interactable = false;
        inputNetwork.text = WalletController.instance.currentNetwork;
        avaliableAddress.SetActive(true);
        invaildAddress.SetActive(false);
        CheckCanAdd(false);
    }
    string WalletFormat(string wallet)
    {
        string _wallet = "";
        _wallet = wallet.Substring(0, 6) + "..." + wallet.Substring(wallet.Length - 4, 4);
        return _wallet;
    }

    public void OnFieldInputWalletAddress()
    {
        //m_WalletAddress = inputAddress.text;
        Debug.Log("OnFieldInputWalletAddress ==================  " + m_WalletAddress);

    }
    public async void OnFieldInputWalletAddressRelease()
    {
        m_WalletAddress = inputAddress.text;
        inputAddress.text = WalletFormat(m_WalletAddress);
        Debug.Log("OnFieldInputWalletAddressRelease ------------------  " + m_WalletAddress);
        Task<bool> tokenAbiTask = WalletController.instance.CheckValidToken(m_WalletAddress);
        bool isValidToken = await tokenAbiTask;
        bool isExistToken = WalletController.instance.CheckExistTokenAdded(m_WalletAddress);
        if (isValidToken && !isExistToken)
        {
            Task<JObject> tokenTask = WalletController.instance.TokenInformation(
                WalletController.instance.wallet_address,
                m_WalletAddress
                );
            m_token = await tokenTask;
            FieldTokenInformation();
            CheckCanAdd(true);
            avaliableAddress.SetActive(true);
            invaildAddress.SetActive(false);
        }
        else
        {
            avaliableAddress.SetActive(false);
            invaildAddress.SetActive(true);
        }
    }
    void FieldTokenInformation()
    {
        Debug.Log("m_token ==== " + m_token);
        inputName.text = m_token["name"].ToString();
        inputSymbol.text = m_token["symbol"].ToString();
        inputDecimals.text = m_token["decimals"].ToString();
    }

    public void OnButtonPaste()
    {
        m_WalletAddress = GUIUtility.systemCopyBuffer;
        inputAddress.text = WalletFormat(m_WalletAddress);
    }
    public void OnButtonScan()
    {
        
    }

    void CheckCanAdd(bool isActived)
    {
        btnAdd.GetComponent<Button>().interactable = isActived;
        btnAdd.GetComponent<Image>().sprite = isActived ? buttonAdds[1] : buttonAdds[0];
    }
    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
    public void OnAddCustomToken()
    {
        WalletController.instance.AddCustomTokenByObject(m_token);
        mainScreenObject.GetComponent<MainScreen>().LoadTokenList();
        gameObject.SetActive(false);

    }
}
