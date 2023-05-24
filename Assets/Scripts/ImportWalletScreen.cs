using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ImportWalletScreen : MonoBehaviour
{
    [SerializeField] private GameObject backScreen;
    [SerializeField] private GameObject setupWalletScreen;

    [SerializeField] private GameObject tagSeedPhrase;
    [SerializeField] private GameObject tagPrivateKey;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private TMP_InputField inputFieldSeed;
    [SerializeField] private TextMeshProUGUI titleInput;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TextMeshProUGUI warningVerify;


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
        Color textColor;
        tagSeedPhrase.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 1 : 0];
        if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 0 : 1], out textColor))
        {
            tagSeedPhrase.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        }

        tagPrivateKey.GetComponent<Image>().sprite = buttonImages[tagIndex == 0 ? 0 : 1];
        if (ColorUtility.TryParseHtmlString(listTagColor[tagIndex == 0 ? 1 : 0], out textColor))
        {
            tagPrivateKey.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        }

        title.text = textTitles[tagIndex];
        description.text = textDescriptions[tagIndex];
        titleInput.text = textTitleInputs[tagIndex];

        placeholder.text = textPlaceHolders[tagIndex];
        inputFieldSeed.text = "";
        warningVerify.text = "";
        m_SeedPhrases = "";
        m_PrivateKey = "";

        CheckCanGoNext(false);
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
        if(tagIndex == 0)
        {
            m_SeedPhrases = inputFieldSeed.text;
            bool isValid = WalletController.instance.VerifySeedPhrase(m_SeedPhrases);
            if (isValid)
            {
                CheckCanGoNext(true);
                warningVerify.text = "";
            }
            else
            {
                CheckCanGoNext(false);
                warningVerify.gameObject.SetActive(true);
                warningVerify.text = textWarnings[0];
            }
        } 
        else
        {
            m_PrivateKey = inputFieldSeed.text;
            bool isValid = WalletController.instance.VerifyWalletByPrivateKey(m_PrivateKey);
            if (isValid)
            {
                CheckCanGoNext(true);
                warningVerify.text = "";
            }
            else
            {
                CheckCanGoNext(false);
                warningVerify.gameObject.SetActive(true);
                warningVerify.text = textWarnings[1];
            }
        }

    }

    public void OnButtonPaste()
    {
        inputFieldSeed.text = GUIUtility.systemCopyBuffer;
    }

    void CheckCanGoNext(bool isActived)
    {
        btnContinue.GetComponent<Button>().interactable = isActived;
        btnContinue.GetComponent<Image>().sprite = isActived ? buttonImages[1] : buttonImages[0];
    }
    public void GoToBackScreen()
    {
        gameObject.SetActive(false);
        backScreen.SetActive(true);
    }
    public void GoToSetupScreen()
    {
        gameObject.SetActive(false);
        setupWalletScreen.SetActive(true);
    }
}
