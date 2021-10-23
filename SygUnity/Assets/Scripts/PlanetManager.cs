using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public GameObject rocketPrefab;
    private int numberRockets;

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
            Debug.Log(go.transform.position);
            Spaceship[] sp = GetComponentsInChildren<Spaceship>();
            foreach (Spaceship s in sp)
            {
                s.StartAttack(go.transform);
            }
        }
    }
}
