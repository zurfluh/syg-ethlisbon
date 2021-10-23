using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float speed;
    public float max;

    void Update()
    {
        float randomX = Random.Range(-speed, speed);
        float randomY = Random.Range(-speed, speed);
        transform.Translate(randomX, randomY, 0);
        Vector3 pos = transform.position;
        if (pos.x >= max) {
            transform.position = new Vector3(max, pos.y, pos.z);
        } else if (pos.x <= -max) {
            transform.position = new Vector3(-max, pos.y, pos.z);
        }
        if (pos.y >= max) {
            transform.position = new Vector3(pos.x, max, pos.z);
        } else if (pos.y <= -max) {
            transform.position = new Vector3(pos.x, -max, pos.z);
        }
    }
}
