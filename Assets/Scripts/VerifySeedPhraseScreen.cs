using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class VerifySeedPhraseScreen : MonoBehaviour
{
    [SerializeField] private GameObject backScreen;
    [SerializeField] private GameObject exploreWalletScreen;
    [SerializeField] private List<TMPro.TextMeshProUGUI> positionPage;
    [SerializeField] private List<TMPro.TMP_InputField> seePhraseByPage;
    [SerializeField] private GameObject messageWarning;


    [SerializeField] private GameObject btnContinue;
    [SerializeField] private List<Sprite> continueImages;

    int currentPage = 0;
    string[] arrayPhrases;
    string[] arrayPhrasesVerify;
    bool m_isCheckedCondition = false;
    // Use this for initialization
    void Start()
    {
        FillSeedPhrase();
    }
    public void FillSeedPhrase()
    {
        arrayPhrases = WalletController.instance.wallet_phrases.Split(char.Parse(" "));
        arrayPhrasesVerify = new string[arrayPhrases.Length];
        for (int i = 0; i < arrayPhrasesVerify.Length; i++)
        {
            arrayPhrasesVerify[i] = "";
        }
        CheckCanGoNext(false);
        SetupPage();
    }

    void SetupPage()
    {
        for (int i = 0; i < positionPage.Count; i++)
        {
            positionPage[i].text = "#"+ (currentPage * positionPage.Count + i + 1);
            seePhraseByPage[i].text = "";
        }
    }
    public void OnFieldSelectedPos_1(TextMeshProUGUI _currentInput)
    {
        arrayPhrasesVerify[currentPage * positionPage.Count] = seePhraseByPage[0].text;
        CheckCanGoNext(CheckActiveButtonContinue());
    }
    public void OnFieldSelectedPos_2(TextMeshProUGUI _currentInput)
    {
        arrayPhrasesVerify[currentPage * positionPage.Count + 1] = seePhraseByPage[1].text;
        CheckCanGoNext(CheckActiveButtonContinue());
    }
    public void OnFieldSelectedPos_3(TextMeshProUGUI _currentInput)
    {
        arrayPhrasesVerify[currentPage * positionPage.Count + 2] = seePhraseByPage[2].text;
        CheckCanGoNext(CheckActiveButtonContinue());
    }
    public void OnFieldSelectedPos_4(TextMeshProUGUI _currentInput)
    {
        arrayPhrasesVerify[currentPage * positionPage.Count + 3] = seePhraseByPage[3].text;
        CheckCanGoNext(CheckActiveButtonContinue());
    }
    bool CheckActiveButtonContinue()
    {
        int count = 0;
        for (int i = 0; i < seePhraseByPage.Count; i++)
        {
            if (seePhraseByPage[i].text != null && seePhraseByPage[i].text != "")
            {
                count++;
            }
        }
        if(count == seePhraseByPage.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckVerifySeedPhrase()
    {
        Debug.Log(" arrayPhrases  =  " + arrayPhrases.ToString());
        Debug.Log(" arrayPhrasesVerify  =  " + arrayPhrasesVerify.ToString());
        int count = 0;
        for (int i = 0; i < seePhraseByPage.Count; i++)
        {
            
            Debug.Log(" arrayPhrasesVerify[currentPage * seePhraseByPage.Count + i] =  " + arrayPhrasesVerify[currentPage * seePhraseByPage.Count + i]);
            Debug.Log(" arrayPhrases[currentPage * seePhraseByPage.Count + i] =  " + arrayPhrases[currentPage * seePhraseByPage.Count + i]);
            if (arrayPhrasesVerify[currentPage * seePhraseByPage.Count + i] == arrayPhrases[currentPage * seePhraseByPage.Count + i])
            {
                count++;
            }
        }
        Debug.Log(" CheckVerifySeedPhrase count =  " + count);
        if (count == seePhraseByPage.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Warning()
    {
        StartCoroutine(ShowMessageWarning());
    }
    IEnumerator ShowMessageWarning()
    {
        messageWarning.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        messageWarning.SetActive(false);
    }


    void CheckCanGoNext(bool isActived)
    {
        btnContinue.GetComponent<Button>().interactable = isActived;
        btnContinue.GetComponent<Image>().sprite = isActived ? continueImages[1] : continueImages[0];
    }
    public void GoToBackScreen()
    {
        if (currentPage == 0)
        {
            gameObject.SetActive(false);
            backScreen.SetActive(true);
        } 
        else
        {
            currentPage--;
            SetupPage();
        }

    }
    public void GoToNextScreen()
    {
        if (CheckVerifySeedPhrase())
        {
            if(currentPage < 2)
            {
                currentPage++;
                SetupPage();
            }
            else
            {
                gameObject.SetActive(false);
                exploreWalletScreen.SetActive(true);
            }
        } 
        else
        {
            Warning();
        }

    }
}
