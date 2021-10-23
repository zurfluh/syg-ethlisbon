using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;
using SygEthlisbon.Contracts.SpaceMafia.ContractDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Unity;

public class PlanetManager : MonoBehaviour
{
    public GameObject rocketPrefab;

    public async Task<System.Numerics.BigInteger> GetStakedEth(System.Numerics.BigInteger planetId)
    {
        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);
        var staked = await spaceMafiaService.StakedEthQueryAsync(planetId);

        return staked;
    }

    public async Task<string> GetOwnerAddress(System.Numerics.BigInteger planetId)
    {
        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);
        var planet = await spaceMafiaService.GetPlanetQueryAsync(planetId);

        return planet.ReturnValue1;
    }

    public async Task ClaimRewards(System.Numerics.BigInteger planetId)
    {
        Debug.Log("rewards");

        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        var claimDividendsFunction = spaceMafiaService.ContractHandler.GetFunction<ClaimDividendsFunction>();

        var address = WalletConnect.ActiveSession.Accounts[0];

        var currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address, BlockParameter.CreatePending());
        Debug.Log("Current nonce: " + currentNonce);

        var transactionInput = claimDividendsFunction.CreateTransactionInput(new ClaimDividendsFunction
        {
            Nonce = currentNonce.Value + 1,
            TokenId = planetId
        }, address);

        Debug.Log("Transaction input: " + transactionInput);

        string rawTx = transactionInput.Data;
        Debug.Log("Raw transaction: " + rawTx);

        var transaction = new TransactionData()
        {
            data = transactionInput.Data,
            from = transactionInput.From,
            to = transactionInput.To,
            gas = "50000",
            value = "0",
            chainId = 5,
            nonce = (currentNonce.Value).ToString(),
            gasPrice = "50000000000"
        };

        WalletConnectUnitySession activeSession = WalletConnect.ActiveSession;
        var results = await activeSession.EthSendTransaction(transaction);
                Debug.Log(results);
    }

    public async Task StakeEther(System.Numerics.BigInteger planetId, float amount)
    {
        Debug.Log("stake ether");

        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        var claimDividendsFunction = spaceMafiaService.ContractHandler.GetFunction<StakeEthOnPlanetFunction>();

        var address = WalletConnect.ActiveSession.Accounts[0];

        var currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address, BlockParameter.CreatePending());
        Debug.Log("Current nonce: " + currentNonce);

        var transactionInput = claimDividendsFunction.CreateTransactionInput(new StakeEthOnPlanetFunction
        {
            Nonce = currentNonce.Value + 1,
            TokenId = planetId
        }, address);

        Debug.Log("Transaction input: " + transactionInput);

        string rawTx = transactionInput.Data;
        Debug.Log("Raw transaction: " + rawTx);

        var transaction = new TransactionData()
        {
            data = transactionInput.Data,
            from = transactionInput.From,
            to = transactionInput.To,
            gas = "50000",
            value = "100000000000000",
            chainId = 5,
            nonce = (currentNonce.Value).ToString(),
            gasPrice = "10000000000"
        };

        WalletConnectUnitySession activeSession = WalletConnect.ActiveSession;
        var results = await activeSession.EthSendTransaction(transaction);
        Debug.Log(results);
    }

    public void AddRocket(System.Numerics.BigInteger planetId)
    {
        Debug.Log("add rocket");

        GameObject go = Instantiate(rocketPrefab, transform);
    }
}
