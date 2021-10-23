using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public GameObject rocketPrefab;

    public float GetStakedEth()
    {
        return Random.Range(0f, 10.0f);
    }

    public string GetOwnerAddress()
    {
        return "0x5e4B3104B8Da480990CD0df17784ae300790AcdB";
    }

    public void ClaimRewards()
    {
        Debug.Log("rewards");
    }

    public void StakeEther(float amount)
    {
        Debug.Log("stake" + amount);
    }

    public void AddRocket()
    {
        GameObject go = Instantiate(rocketPrefab, transform);
    }
}
