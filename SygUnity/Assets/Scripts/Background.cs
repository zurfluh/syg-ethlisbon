using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed = -0.3f;
    public float limit = -32f;
    public float startX;

    void Start()
    {
        startX = transform.localPosition.x;
    }

    void Update()
    {
        if (transform.localPosition.x < limit)
        {
            transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);
        }
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
