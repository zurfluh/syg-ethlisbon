using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System.Diagnostics;
using System.Threading.Tasks;

public class UpdatePlanetInfoCoroutine : MonoBehaviour
{
    public string UrlFull = "https://mainnet.infura.io/v3/8641c45d947a4e159304a56bdad174e2";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    public Text planetName;
    public Text EthAddress;
    public Text StakeAmount;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = UrlFull;
    }


    public IEnumerator UpdatePlanetInfo(System.Numerics.BigInteger planetId, PlanetManager pm, string selectedName)
    {
        yield return update(planetId, pm, selectedName);
    }

    private async Task update(System.Numerics.BigInteger planetId, PlanetManager pm, string selectedName)
    {
        //System.Numerics.BigInteger eth = await pm.GetStakedEth(planetId);

        // UI Texts
        EthAddress.text = await pm.GetOwnerAddress(planetId);
        planetName.text = selectedName;
        //StakeAmount.text = eth.ToString();

        // Size
        //int index = Mathf.FloorToInt((float)eth);
        //child.transform.localScale = sizes[index];

        //SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
        //float r = 0.3f + 0.03f * index;
        //float g = 0.1f * index;
        //float b = 0.2f + 0.06f * index;
        //Color color = new Color(r, g, b);
        //sr.color = color;
    }
}
