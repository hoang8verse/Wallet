using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class SetupWalletScreen : MonoBehaviour
{
    [SerializeField] private GameObject backScreen;
    [SerializeField] private GameObject exploreWalletScreen;
    [SerializeField] private TMP_InputField walletName;
    [SerializeField] private TMP_InputField walletPassword;
    [SerializeField] private TMP_InputField walletPasswordVerify;

    [SerializeField] private Image iconCondition_1;
    [SerializeField] private TextMeshProUGUI textCondition_1;
    [SerializeField] private Image iconCondition_2;
    [SerializeField] private TextMeshProUGUI textCondition_2;
    [SerializeField] private List<Sprite> verifyIconImages;

    [SerializeField] private GameObject warningVerifyPassword;

    [SerializeField] private Button btnShowPassword;
    [SerializeField] private Button btnShowPasswordVerify;

    [SerializeField] private GameObject btnContinue;
    [SerializeField] private List<Sprite> continueImages;

    string[] listVerifyColor = new string[] { "#5A7DBD", "#FC4F4F", "#00AA30" }; // normal, fail, success
    string m_password = "";

    // Use this for initialization
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        walletPassword.contentType = TMP_InputField.ContentType.Password;
        walletPasswordVerify.contentType = TMP_InputField.ContentType.Password;
        SetCondition_1Status(0);
        SetCondition_2Status(0);
    }

    void SetCondition_1Status(int status)
    {
        iconCondition_1.sprite = verifyIconImages[status];
        Color textColor;
        if (ColorUtility.TryParseHtmlString(listVerifyColor[status], out textColor))
        {
            textCondition_1.color = textColor;
        }
    }
    void SetCondition_2Status(int status)
    {
        iconCondition_2.sprite = verifyIconImages[status];
        Color textColor;
        if (ColorUtility.TryParseHtmlString(listVerifyColor[status], out textColor))
        {
            textCondition_2.color = textColor;
        }
    }

   public void SetStatusTypePassword()
    {
        if(walletPassword.contentType == TMP_InputField.ContentType.Password)
        {
            walletPassword.contentType = TMP_InputField.ContentType.Standard;
        } 
        else if (walletPassword.contentType == TMP_InputField.ContentType.Standard)
        {
            walletPassword.contentType = TMP_InputField.ContentType.Password;
        }
        walletPassword.ForceLabelUpdate();
    }

    public void SetStatusTypePasswordVerify()
    {
        if (walletPasswordVerify.contentType == TMP_InputField.ContentType.Password)
        {
            walletPasswordVerify.contentType = TMP_InputField.ContentType.Standard;
        }
        else if (walletPasswordVerify.contentType == TMP_InputField.ContentType.Standard)
        {
            walletPasswordVerify.contentType = TMP_InputField.ContentType.Password;
        }
        walletPasswordVerify.ForceLabelUpdate();
    }

    public void OnFieldSelectedName()
    {
        WalletController.instance.wallet_name = walletName.text;
    }
    public void OnFieldSelectedPassword()
    {
        m_password = walletPassword.text;
        if(PasswordVerifier.instance.VerifyPasswordLength(walletPassword.text))
        {
            SetCondition_1Status(2); // success
        } 
        else
        {
            SetCondition_1Status(1); // fail
        }

        if (PasswordVerifier.instance.VerifyPasswordPattern(walletPassword.text))
        {
            SetCondition_2Status(2); // success
        }
        else
        {
            SetCondition_2Status(1); // fail
        }
    }
    public void OnFieldSelectedPasswordVerify()
    {
        if(m_password != walletPasswordVerify.text)
        {
            warningVerifyPassword.SetActive(true);
        }
        else
        {
            warningVerifyPassword.SetActive(false);
            CheckCanGoNext(true);
        }
        
    }
    void CheckCanGoNext(bool isActived)
    {
        btnContinue.GetComponent<Button>().interactable = isActived;
        btnContinue.GetComponent<Image>().sprite = isActived ? continueImages[1] : continueImages[0];
    }
    public void GoToBackScreen()
    {
        gameObject.SetActive(false);
        backScreen.SetActive(true);
    }
    public void GoToExploreScreen()
    {
        WalletController.instance.wallet_password = m_password;
        WalletController.instance.SetupWallet();
        gameObject.SetActive(false);
        exploreWalletScreen.SetActive(true);
    }
}
