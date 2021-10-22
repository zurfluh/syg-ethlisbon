using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System.Diagnostics;

public class GetLatestBlockCoroutine : MonoBehaviour
{
    // public string Url = "https://ropsten.infura.io";
    public string UrlFull = "https://mainnet.infura.io/v3/8641c45d947a4e159304a56bdad174e2";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        // InputUrl.text = Url;
    }

    public void GetBlockNumberRequest()
    {
        UnityEngine.Debug.Log("aaa");
        StartCoroutine(GetBlockNumber());
        UnityEngine.Debug.Log("zzz");
    }

    public IEnumerator GetBlockNumber()
    {
        var blockNumberRequest = new EthBlockNumberUnityRequest(UrlFull);
        yield return blockNumberRequest.SendRequest();
        UnityEngine.Debug.Log("start");

        if (blockNumberRequest.Exception != null)
        {
            UnityEngine.Debug.Log(blockNumberRequest.Exception.Message);
        }
        else
        {
            ResultBlockNumber.text = blockNumberRequest.Result.Value.ToString();
        }
        UnityEngine.Debug.Log("end");
    }
}
