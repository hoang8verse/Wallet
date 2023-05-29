using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private GameObject setupScreen;
    [SerializeField] private GameObject exploreScreen;
    
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CheckNextScreen());
    }

    IEnumerator CheckNextScreen()
    {
        bool isGotoExploreWallet = true;
        if(!ProfileManager.Instance.CheckExistProfile())
        {
            isGotoExploreWallet = false;
        }

        yield return new WaitForSeconds(1.0f);
        if (isGotoExploreWallet)
        {
            GoToExploreScreen();
            ProfileManager.Instance.ParseProfile();
        }
        else
        {
            GoToSetupScreen();
        }
        
    }

    private void GoToSetupScreen()
    {
        gameObject.SetActive(false);
        setupScreen.SetActive(true);
    }

    private void GoToExploreScreen()
    {
        gameObject.SetActive(false);
        exploreScreen.SetActive(true);
    }

}
