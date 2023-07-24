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
using Nethereum.Hex.HexTypes;

public class TokenSending : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFrom;
    [SerializeField] private TMP_InputField inputAmount;
    [SerializeField] private TMP_InputField inputTo;

    [SerializeField] private Slider sliderEstimateGasFee;

    [SerializeField] private GameObject objectEstimateGasFee;
    [SerializeField] private GameObject objectConfirmOrder;
    [SerializeField] private GameObject objectVerifyTransaction;

    [SerializeField] private TextMeshProUGUI estimateGasValue;

    [SerializeField] private TextMeshProUGUI orderAmountTitle;
    [SerializeField] private TextMeshProUGUI orderAddressFrom;
    [SerializeField] private TextMeshProUGUI orderAddressTo;
    [SerializeField] private TextMeshProUGUI orderAmountValue;
    [SerializeField] private TextMeshProUGUI orderFee;
    [SerializeField] private TextMeshProUGUI orderTotal;

    string m_fromAddress = "";
    string m_toAddress = "";
    HexBigInteger m_amount;
    HexBigInteger m_gasPrice;
    // Use this for initialization
    void Start()
    {
        Setup();
    }
    void Setup()
    {
        m_fromAddress = WalletController.instance.wallet_address;
        inputFrom.text = WalletFormat(m_fromAddress);
    }
    string WalletFormat(string wallet)
    {
        string _wallet = "";
        _wallet = wallet.Substring(0, 6) + "..." + wallet.Substring(wallet.Length - 4, 4);
        return _wallet;
    }

    public void OnFieldInputToAddress()
    {
        m_toAddress = inputTo.text;
    }
    public void OnFieldInputAmount()
    {
        m_amount = new HexBigInteger(inputAmount.text);
    }
    public void OnMaxAmount()
    {
        
    }

    public void OnSetGasPrice()
    {
        Debug.Log(" sliderEstimateGasFee ===" + sliderEstimateGasFee.value);
    }

    public void OnFieldInputVerifyPassword()
    {
        
    }

    public void GotoNext()
    {
        objectEstimateGasFee.SetActive(true);
    }
    public void EstimateGasFee()
    {
        objectEstimateGasFee.SetActive(false);
        objectConfirmOrder.SetActive(true);
    }
    public void ConfirmOrder()
    {
        objectConfirmOrder.SetActive(false);
        objectVerifyTransaction.SetActive(true);
    }
    public void VerifyTransaction()
    {
        objectVerifyTransaction.SetActive(false);
    }
}
