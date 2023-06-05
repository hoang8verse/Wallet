using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;

public class MainScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI walletName;
    [SerializeField] private TextMeshProUGUI walletAddress;
    [SerializeField] private TextMeshProUGUI totalPrice;

    [SerializeField] private GameObject addCustomTokenObject;
    [SerializeField] private Button btnAddCustomToken;

    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] ScrollRect tokenList;

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
        walletAddress.text = WalletFormat(WalletController.instance.wallet_address);
        totalPrice.text = "$ " + 0.ToString("N3");
        LoadTokenList();
    }
    public void LoadTokenList()
    {
        Transform contentTransform = tokenList.content;
        
        // Iterate through all child objects
        if(contentTransform.childCount > 0)
        {
            for (int i = contentTransform.childCount - 1; i >= 0; i--)
            {
                // Destroy each child object
                Destroy(contentTransform.GetChild(i).gameObject);
            }
        }
       
        JArray listTokens = WalletController.instance.listTokens;
        for (int i = 0; i < listTokens.Count; i++)
        {
            GameObject instance = Instantiate(tokenPrefab, tokenList.content);
            instance.GetComponent<TokenInfo>().SetupToken(
                listTokens[i]["name"].ToString(),
                (decimal)listTokens[i]["balance"],
                listTokens[i]["symbol"].ToString(),
                0,
                0,
                0
                );
            instance.name = listTokens[i]["symbol"].ToString();

        }
    }

    string WalletFormat(string wallet)
    {
        string _wallet = "";
        _wallet = wallet.Substring(0,6) + "..." + wallet.Substring(wallet.Length - 4,4);
        return _wallet;
    }

    public void OnSelectTag(int index)
    {
        if(tagIndex != index)
        {
            tagIndex = index;
            SetupTag();
        }

    }

    public void OnButtonAddToken()
    {
        addCustomTokenObject.SetActive(true);
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
