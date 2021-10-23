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
        distance = Random.Range(1.0f, 4.0f);
    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
