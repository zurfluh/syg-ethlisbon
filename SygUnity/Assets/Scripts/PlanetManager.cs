using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;
using SygEthlisbon.Contracts.SpaceMafia.ContractDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

    public async Task ClaimRewards(System.Numerics.BigInteger planetId)
    {
        Debug.Log("rewards");

        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        var claimDividendsFunction = spaceMafiaService.ContractHandler.GetFunction<ClaimDividendsFunction>();

        var currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync("0x54782", BlockParameter.CreatePending());
        Debug.Log("Current nonce: " + currentNonce);

        var transactionInput = claimDividendsFunction.CreateTransactionInput(new ClaimDividendsFunction
        {
            Nonce = currentNonce.Value + 1,
            TokenId = planetId
        }, "0x0");

        Debug.Log("Transaction input: " + transactionInput);

        string rawTx = transactionInput.Data;
        Debug.Log("Raw transaction: " + rawTx);
        // TODO: Sign and send trx by external wallet.
    }

    public async Task StakeEther(System.Numerics.BigInteger planetId, float amount)
    {
        Debug.Log("stake" + amount);
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
