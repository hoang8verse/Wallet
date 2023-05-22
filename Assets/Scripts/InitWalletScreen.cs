using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;


public class InitWalletScreen : MonoBehaviour
{
    [SerializeField] private GameObject createScreen;
    [SerializeField] private GameObject importScreen;

    // Use this for initialization
    void Start()
    {

    }
    public void GoToCreateScreen()
    {
        WalletController.instance.OnCreateWallet();
        gameObject.SetActive(false);
        createScreen.SetActive(true);
    }
    public void GoToImportScreen()
    {
        gameObject.SetActive(false);
        importScreen.SetActive(true);
    }
}
