using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;
using SygEthlisbon.Contracts.SpaceMafia.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

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

        var claimDividendsFunction = spaceMafiaService.ContractHandler.GetFunction<ClaimDividendsFunction>();

        var currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync("0x54782", BlockParameter.CreatePending());
        Debug.Log("Current nonce: " + currentNonce);
        
        var transactionInput = claimDividendsFunction.CreateTransactionInput(new ClaimDividendsFunction
        {
            Nonce = currentNonce.Value + 1,
            TokenId = 45678
        }, "0x123456765434567");

        Debug.Log("Transaction input: " + transactionInput);

        string rawTx = transactionInput.Data;
        Debug.Log("Raw transaction: " + rawTx);
        // TODO: Sign and send trx by external wallet.
    }
}
