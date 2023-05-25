
using UnityEngine;


public class ExploreScreen : MonoBehaviour
{
    [SerializeField] private GameObject setupFlow;
    [SerializeField] private GameObject walletFlow;

    // Use this for initialization
    void Start()
    {
    }
    public void GoToNextScreen()
    {
        gameObject.SetActive(false);
        setupFlow.SetActive(false);
        walletFlow.SetActive(true);
    }

}
