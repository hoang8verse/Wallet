using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;


public class SeedPhraseScreen : MonoBehaviour
{
    [SerializeField] private GameObject backScreen;
    [SerializeField] private GameObject verifySeedPhraseScreen;
    [SerializeField] private List<TMPro.TextMeshProUGUI> listPhrase;

    [SerializeField] private GameObject btnCondition;
    [SerializeField] private List<Sprite> conditionImages;
    [SerializeField] private GameObject btnContinue;
    [SerializeField] private List<Sprite> continueImages;

    bool m_isCheckedCondition = false;
    // Use this for initialization
    void Start()
    {
        FillSeedPhrase();
    }
    public void FillSeedPhrase()
    {
        string[] splitArrayPhrases = WalletController.instance.wallet_phrases.Split(char.Parse(" "));

        for (int i = 0; i < listPhrase.Count; i++)
        {
            listPhrase[i].text = splitArrayPhrases[i];
        }
        CheckCanGoNext(false);
    }
    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = WalletController.instance.wallet_phrases;
    }
    public void UnderstandCondition()
    {
        m_isCheckedCondition = !m_isCheckedCondition;
        CheckCanGoNext(m_isCheckedCondition);
    }

    void CheckCanGoNext(bool isActived)
    {
        btnCondition.GetComponent<Image>().sprite = isActived ? conditionImages[1] : conditionImages[0];

        btnContinue.GetComponent<Button>().interactable = isActived;
        btnContinue.GetComponent<Image>().sprite = isActived ? continueImages[1] : continueImages[0];
    }
    public void GoToBackScreen()
    {
        gameObject.SetActive(false);
        backScreen.SetActive(true);
    }
    public void GoToVerifyScreen()
    {
        gameObject.SetActive(false);
        verifySeedPhraseScreen.SetActive(true);
    }
}
