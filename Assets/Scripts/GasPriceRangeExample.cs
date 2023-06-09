using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Numerics;
using Newtonsoft.Json;
using Nethereum.Hex.HexTypes;

public class GasPriceRangeExample : MonoBehaviour
{
    private IEnumerator Start()
    {
        string bscExplorerAPIUrl = "https://api-testnet.bscscan.com/api"; // Replace with the BSC testnet explorer API URL

        string module = "proxy";
        string action = "eth_gasPrice";

        string requestUrl = $"{bscExplorerAPIUrl}?module={module}&action={action}";

        UnityWebRequest request = UnityWebRequest.Get(requestUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log("responseJson   : " + responseJson);
            //GasPriceResponse gasPriceResponse = JsonUtility.FromJson<GasPriceResponse>(responseJson);

            dynamic apiResponse = JsonConvert.DeserializeObject(responseJson);

            if (apiResponse != null)
            {
                string gasPriceHex = apiResponse["result"];
                HexBigInteger gasPrice = new HexBigInteger(gasPriceHex);

                Debug.Log("Gas Price: " + gasPrice);
            }
           
        }
        else
        {
            Debug.LogError("Failed to get gas price: " + request.error);
        }
    }

    [System.Serializable]
    private class GasPriceResponse
    {
        public string id;
        public string message;
        public string result;
    }
}
