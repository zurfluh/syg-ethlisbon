using Nethereum.Web3;
using SygEthlisbon.Contracts.SpaceMafia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Manager : MonoBehaviour
{
    public Text planetName;
    public Text ethAddress;
    public Dropdown dropdown;
    public Text StakeAmount;
    private string ethAddressString = "";
    private float stakedEthFloat;

    private GameObject selected;
    private PlanetManager selectedPM;
    private string selectedName;
    private int selectedOffset;
    private Vector3[] sizes;
    private System.Numerics.BigInteger basePlanetId;


    private Dictionary<string, int> planetOffsets = new Dictionary<string, int>
    {
        { "Bob", 1 },
        { "Satoland", 2 },
        { "Aurum", 3 },
        { "Magnus", 4 },
        { "Helius", 5 },
    };
    
    async Task Awake()
    {
        selectedName = "";
        sizes = new Vector3[10];
        for (int i = 0; i < sizes.Length; i++)
        {
            float f = 0.25f + i * 0.07f;
            sizes[i] = new Vector3(f, f, f);
        }

        // Get Planet IDs
        var web3 = new Web3(GameManager.Instance.InfuraUrl);
        var spaceMafiaService = new SpaceMafiaService(web3, GameManager.Instance.SpaceMafiaContractAddress);

        basePlanetId = await spaceMafiaService.PlanetTypeQueryAsync();
    }

    void Update()
    {
        ethAddress.text = ethAddressString;
        StakeAmount.text = stakedEthFloat.ToString();
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.name != selectedName) {
                if (selected != null) {
                    selected.transform.GetChild(0).gameObject.SetActive(false);
                }
                selected = hit.collider.gameObject;
                selectedName = selected.name;
                selectedOffset = planetOffsets[selectedName];
                Transform child = selected.transform.GetChild(0);
                child.gameObject.SetActive(true);

                PlanetManager pm = selected.gameObject.GetComponent<PlanetManager>();
                this.selectedPM = pm;
                System.Numerics.BigInteger planetId = basePlanetId + selectedOffset;
                planetName.text = selectedName;
                StartCoroutine(UpdatePlanetInfo(planetId, pm, selectedName));

                //System.Numerics.BigInteger eth = await pm.GetStakedEth(planetId);
                                
                //// UI Texts
                //planetOwner.text = await pm.GetOwnerAddress(planetId);
                //StakeAmount.text = eth.ToString();

                //// Size
                //int index = Mathf.FloorToInt((float)eth);
                //child.transform.localScale = sizes[index];

                // Color
                //SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
                //float r = 0.3f + 0.03f * index;
                //float g = 0.1f * index;
                //float b = 0.2f + 0.06f * index;
                //Color color = new Color(r, g, b);
                //sr.color = color;
            }
        }
    }

    public IEnumerator UpdatePlanetInfo(System.Numerics.BigInteger planetId, PlanetManager pm, string selectedName)
    {
        Task task =  update(planetId, pm, selectedName);
        yield return new WaitUntil(() => task.IsCompleted);
    }

    private async Task update(System.Numerics.BigInteger planetId, PlanetManager pm, string selectedName)
    {
        // Staked ETH
        System.Numerics.BigInteger stakedEth = await pm.GetStakedEth(planetId);
        // Step 1: bring the number down to a small number so it can be managed as a float
        stakedEthFloat = (float)(stakedEth / System.Numerics.BigInteger.Pow(10, 14));
        // Compute the exact amount as a float
        stakedEthFloat = stakedEthFloat / 10000;

        // Get Address
        ethAddressString = await pm.GetOwnerAddress(planetId);

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



    public async Task ClaimRewards()
    {
        System.Numerics.BigInteger planetId = basePlanetId + selectedOffset;
        StartCoroutine(ClaimRewardsCoroutine(planetId));
    }

    public IEnumerator ClaimRewardsCoroutine(System.Numerics.BigInteger planetId)
    {
        yield return this.selectedPM.ClaimRewards(planetId);
    }

    public void StakeEther(float amount)
    {
        System.Numerics.BigInteger planetId = basePlanetId + selectedOffset;
        StartCoroutine(StakeEtherCoroutine(planetId));
    }

    public IEnumerator StakeEtherCoroutine(System.Numerics.BigInteger planetId)
    {
        yield return this.selectedPM.StakeEther(planetId, 1f);
    }

    public async void AddRocket()
    {
        System.Numerics.BigInteger planetId = basePlanetId + selectedOffset;
        this.selectedPM.AddRocket(planetId);
        await this.selectedPM.MintRocket(planetId);
    }

    public void Attack()
    {
        this.selectedPM.Attack(dropdown.options[dropdown.value].text);
        var attackingPlanet = planetOffsets[dropdown.options[dropdown.value].text];
        var attackingRocket = rocketOffsets[dropdown.options[dropdown.value].text];
        var missionCost = 500000000;
        this.selectedPM.Attacking(attackingPlanet, attackingRocket, missionCost);
    }
}
