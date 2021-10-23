using System.Collections;
using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;
using UnityEngine;

namespace Assets.Scripts
{
    public class GetPlanetInfo : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        public async void GetPlanetInfoAsync()
        {
            var web3 = new Web3(GameManager.Instance.InfuraUrl);
            var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);
            var result = await spaceMafiaService.PlanetTypeQueryAsync();
        }
    }
}