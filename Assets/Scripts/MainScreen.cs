using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class MainScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI walletName;
    [SerializeField] private TextMeshProUGUI walletAddress;


    [SerializeField] private Button btnPaste;

    [SerializeField] private GameObject btnContinue;
    [SerializeField] private List<Sprite> buttonImages;

    string[] listTagColor = new string[] { "#FFFFFF", "#2954A3" }; 

    string[] textTitles = new string[] {
        "Import a wallet using Seed Phrase",
        "Import a wallet using Private Key"
    };
    string[] textDescriptions = new string[] {
        "Typing or paste 12 (sometime 24) seed phrase here to restore your wallet",
        "Enter the private key here to restore your wallet"
    };
    string[] textTitleInputs = new string[] {
        "Seed Phrase",
        "Private Key"
    };
    string[] textPlaceHolders = new string[] {
        "Enter seed phrase",
        "Enter private key"
    };
    string[] textWarnings = new string[] {
        "Invalid Seed Phrase",
        "Invalid Private Key"
    };
    int tagIndex = 0;
    string m_SeedPhrases = "";
    string m_PrivateKey = "";
    // Use this for initialization
    void Start()
    {
        SetupTag();
    }

    void SetupTag()
    {
        //Color textColor;
        //tagSeedPhrase.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 1 : 0];
        //if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 0 : 1], out textColor))
        //{
        //    tagSeedPhrase.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        //}

        //tagPrivateKey.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 0 : 1];
        //if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 1 : 0], out textColor))
        //{
        //    tagPrivateKey.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        //}


        walletName.text = WalletController.instance.wallet_name;
        walletAddress.text = WalletController.instance.wallet_address;



    }

    public void OnSelectTag(int index)
    {
        if(tagIndex != index)
        {
            tagIndex = index;
            SetupTag();
        }

    }
    public void OnFieldInputWallet()
    {
        

    }

    public void OnButtonCopyAddress()
    {
        walletAddress.text = GUIUtility.systemCopyBuffer;
    }


    public void GoToBackScreen()
    {
        gameObject.SetActive(false);

    }
    public void GoToSetupScreen()
    {
        gameObject.SetActive(false);
    }
}
