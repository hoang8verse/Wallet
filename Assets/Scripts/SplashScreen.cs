using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine;


public class SplashScreen : MonoBehaviour
{
    [SerializeField] private GameObject nextScreen;

    // Use this for initialization
    void Start()
    {
        Invoke("GoToNextScreen", 1.0f);
    }
    private void GoToNextScreen()
    {
        gameObject.SetActive(false);
        nextScreen.SetActive(true);
    }

}
