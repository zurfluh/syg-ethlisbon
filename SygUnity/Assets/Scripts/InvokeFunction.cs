using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;

public class InvokeFunction : MonoBehaviour
{
    public string UrlFull = "https://mainnet.infura.io/v3/8641c45d947a4e159304a56bdad174e2";
    public string fromAddress = "0x123456";
    public string contractAddress = "0x1234";
    public string functionName = "attackPlanet";
    public string abi = @"[{'constant':false,'inputs':[{'name':'a','type':'int256'}],'name':'multiply','outputs':[{'name':'r','type':'int256'}],'type':'function'},{'inputs':[{'name':'multiplier','type':'int256'}],'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'a','type':'int256'},{'indexed':true,'name':'sender','type':'address'},{'indexed':false,'name':'result','type':'int256'}],'name':'Multiplied','type':'event'}]";
    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = UrlFull;
    }

    public async void CallSomeFunction()
    {
        var web3 = new Web3("https://goerli.infura.io");
        var contract = web3.Eth.GetContract(abi, contractAddress);
        var function = contract.GetFunction(functionName);

        var parameters = new object[]
        {
            new
            {
                Data = 424242,
                To = "0x78975456"
            }
        };

        var txHash = await function.SendTransactionAsync(fromAddress, parameters);
        Debug.Log("Transaction hash for function call: " + txHash);
    }
}
