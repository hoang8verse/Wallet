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
    [SerializeField] private GameObject tagTokens;
    [SerializeField] private GameObject tagNFTs;

    [SerializeField] private TextMeshProUGUI walletName;
    [SerializeField] private TextMeshProUGUI walletAddress;
    [SerializeField] private TextMeshProUGUI totalPrice;

    [SerializeField] private TextMeshProUGUI searchHolder;

    [SerializeField] private GameObject addCustomTokenObject;
    [SerializeField] private GameObject addCustomNFTObject;
    [SerializeField] private Button btnAddCustomToken;

    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] ScrollRect tokenList;

    [SerializeField] private GameObject NFTPrefab;
    [SerializeField] ScrollRect NFTList;

    [SerializeField] private List<Sprite> buttonImages;

    string[] listTagColor = new string[] { "#FFFFFF", "#2954A3" }; 

    string[] textSearchHolder = new string[] {
        "Search tokens",
        "Search NFTs"
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
        Color textColor;
        tagTokens.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 1 : 0];
        if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 0 : 1], out textColor))
        {
            tagTokens.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        }

        tagNFTs.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 0 : 1];
        if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 1 : 0], out textColor))
        {
            tagNFTs.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        }

        searchHolder.text = textSearchHolder[tagIndex];

        walletName.text = WalletController.instance.wallet_name;
        walletAddress.text = WalletFormat(WalletController.instance.wallet_address);
        totalPrice.text = "$ " + 0.ToString("N3");
        if(tagIndex == 0)
        {
            NFTList.gameObject.SetActive(false);
            tokenList.gameObject.SetActive(true);
            LoadTokenList();
        }
        else
        {
            NFTList.gameObject.SetActive(true);
            tokenList.gameObject.SetActive(false);
            LoadNFTsList();
        }
        
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
    public void LoadNFTsList()
    {
        Transform contentTransform = NFTList.content;

        // Iterate through all child objects
        if (contentTransform.childCount > 0)
        {
            for (int i = contentTransform.childCount - 1; i >= 0; i--)
            {
                // Destroy each child object
                Destroy(contentTransform.GetChild(i).gameObject);
            }
        }

        JArray listNFTs = WalletController.instance.listNFTs;
        if(listNFTs.Count > 0)
        {
            for (int i = 0; i < listNFTs.Count; i++)
            {
                GameObject instance = Instantiate(NFTPrefab, NFTList.content);
                instance.GetComponent<NFTInfo>().SetupNFT(
                    listNFTs[i]["name"].ToString(),
                    (decimal)listNFTs[i]["balance"],
                    listNFTs[i]["symbol"].ToString()
                    );
                instance.name = listNFTs[i]["symbol"].ToString();

            }
        }
        else
        {

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
        if (tagIndex == 0)
        {
            addCustomTokenObject.SetActive(true);
            addCustomNFTObject.SetActive(false);
        } 
        else
        {
            addCustomNFTObject.SetActive(true);
            addCustomTokenObject.SetActive(false);
        }
        
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
