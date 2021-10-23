using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;

public class ClaimRewards : MonoBehaviour
{  
    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        //
    }

    public async void CallSomeFunction()
    {
        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        // TODO: Token ID as input.
        var txHash = await spaceMafiaService.ClaimDividendsRequestAsync(123456);
        Debug.Log("Transaction hash for function call: " + txHash);
    }
}
