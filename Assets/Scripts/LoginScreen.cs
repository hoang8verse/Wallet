using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class LoginScreen : MonoBehaviour
{
    [SerializeField] private GameObject setupFlow;
    [SerializeField] private GameObject walletFlow;

    [SerializeField] private GameObject nextUnlockScreen;

    [SerializeField] private GameObject nextResetScreenCondition;
    [SerializeField] private GameObject nextResetScreenConfirm;

    [SerializeField] private TMP_InputField inputFieldPassword;
    [SerializeField] private TextMeshProUGUI warningVerify;

    [SerializeField] private Button btnFaceId;
    [SerializeField] private Button btnReset;

    [SerializeField] private GameObject btnUnlock;

    [SerializeField] private TMP_InputField inputFieldReset;

    string m_Password = "";
    bool m_isCorrectPassword = false;
    bool m_isResetSuccess = false;
    // Use this for initialization
    void Start()
    {
        nextResetScreenCondition.SetActive(false);
        nextResetScreenConfirm.SetActive(false);
    }
        
    public void OnFieldInputPassword()
    {
        m_Password = inputFieldPassword.text;
        if(m_Password == "password")
        {
            m_isCorrectPassword = true;
            warningVerify.gameObject.SetActive(false);
        }
        else
        {
            m_isCorrectPassword = false;
            warningVerify.gameObject.SetActive(true);
        }

    }
    public void OnFieldInputReset()
    {
        if(inputFieldReset.text == "Reset")
        {
            m_isResetSuccess = true;
        }
        else
        {
            m_isResetSuccess = false;
        }
        

    }

    public void OnButtonFaceId()
    {

    }
    public void OnButtonReset()
    {
        nextResetScreenCondition.SetActive(true);
    }
    public void OnButtonResetCondition()
    {
        nextResetScreenCondition.SetActive(false);
        nextResetScreenConfirm.SetActive(true);
    }
    public void OnButtonCancelCondition()
    {
        nextResetScreenCondition.SetActive(false);
    }
    public void OnButtonResetConfirm()
    {
        if (m_isResetSuccess)
        {
            nextResetScreenCondition.SetActive(false);
            nextResetScreenConfirm.SetActive(false);
            gameObject.SetActive(false);
            setupFlow.SetActive(true);
            walletFlow.SetActive(false);
        }

    }
    public void OnButtonCancelConfirm()
    {
        nextResetScreenConfirm.SetActive(false);
    }

    public void GoToNextScreen()
    {
        if (m_isCorrectPassword)
        {
            gameObject.SetActive(false);
        }
        
    }
}
