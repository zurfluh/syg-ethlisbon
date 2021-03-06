using System.Collections;
using System.Collections.Generic;
using Nethereum.Web3;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string InfuraUrl { get; set; }
    public string GalaxyTokenContractAddress { get; set; }
    public string SpaceMafiaContractAddress { get; set; }

    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }
}
