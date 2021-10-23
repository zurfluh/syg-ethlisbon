using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    float speed;
    float distance;

    void Start()
    {
        speed = Random.Range(25.0f, 40.0f);
        distance = Random.Range(0.8f, 3f);
        transform.GetChild(0).localPosition = new Vector3(distance, 0, 0);
    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
