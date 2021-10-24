using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using SygEthlisbon.Contracts.SpaceMafia;
using SygEthlisbon.Contracts.SpaceMafia.ContractDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;

public class PlanetManager : MonoBehaviour
{
    public GameObject rocketPrefab;
    private int numberRockets;

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

    public async Task<System.Numerics.BigInteger> GetMafiaBalance(string account){
        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);
        var mafiaBalance = await spaceMafiaService.GetMafiaBalanceOfQueryAsync(account);
        return mafiaBalance;
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

    public async Task MintRocket(System.Numerics.BigInteger planetId){
        
        Debug.Log("mint rocket");
        var account = new Account("d7f003b771626dca712ef06f602ee0f076b98917e528d4de188790036589e0be");
        var web3 = new Web3(account, GameManager.Instance.InfuraUrl);
        var mintRocket = web3.Eth.GetContractTransactionHandler<MintRocketFunction>();

        var rocketMinter = new MintRocketFunction(){
         PlanetId = planetId
        };
        Debug.Log("PlanetId"+ planetId);
        // var address = WalletConnect.ActiveSession.Accounts[0];
        var transactionReceipt = await mintRocket.SendRequestAndWaitForReceiptAsync(GameManager.Instance.SpaceMafiaContractAddress,rocketMinter);
        Debug.Log(transactionReceipt);
    }

    public async Task Attacking(System.Numerics.BigInteger planetId, System.Numerics.BigInteger rocketId, System.Numerics.BigInteger missionCost){
         Debug.Log("stake ether");

        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        var nukeFunction = spaceMafiaService.ContractHandler.GetFunction<NukeFunction>();

        var address = WalletConnect.ActiveSession.Accounts[0];

        var currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address, BlockParameter.CreatePending());
        Debug.Log("Current nonce: " + currentNonce);

        var transactionInput = nukeFunction.CreateTransactionInput(new NukeFunction
        {
            Nonce = currentNonce.Value + 1,
            PlanetId = planetId,
            RocketId = rocketId,
            MissionCost = missionCost,
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
        SpriteRenderer sr = go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        float r = 0.1f + Random.Range(0, 0.5f);;
        float g = 0.3f + Random.Range(0, 0.75f);;
        float b = 0.3f + Random.Range(0, 0.75f);;
        Color color = new Color(r, g, b);
        sr.color = color;
        numberRockets++;
    }

    public void Attack(string planetName)
    {
        if (numberRockets > 0)
        {
            GameObject go = GameObject.Find(planetName);
            Spaceship[] sp = GetComponentsInChildren<Spaceship>();
            foreach (Spaceship s in sp)
            {
                s.StartAttack(go.transform);
            }
        }
    }
}
