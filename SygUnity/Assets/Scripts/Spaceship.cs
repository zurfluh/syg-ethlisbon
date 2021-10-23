using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    enum State
    {
        Rotate, Attack, Destroy
    }

    private float speed;
    private float distance;
    private State state;
    private Vector3 attackSource;
    private Transform attackDestination;
    private Transform spaceshipSprite;
    private float startTime;
    private float attackLength;
    private float attackSpeed;
    
    public GameObject explosionPrefab;

    void Start()
    {
        state = State.Rotate;
        speed = Random.Range(25.0f, 40.0f);
        attackSpeed = Random.Range(0.7f, 1.2f);
        distance = Random.Range(0.8f, 2f);
        spaceshipSprite = transform.GetChild(0);
        spaceshipSprite.localPosition = new Vector3(distance, 0, 0);
    }

    public void StartAttack(Transform destination)
    {
        state = State.Attack;
        attackSource = spaceshipSprite.position;
        attackDestination = destination;
        startTime = Time.time;
        attackLength = Vector3.Distance(attackSource, attackDestination.position);

        Transform grandparent = transform.parent.parent;
        transform.parent = grandparent;
    }
    
    void Update()
    {
        if (state == State.Rotate)
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        else if (state == State.Attack)
        {
            float distCovered = (Time.time - startTime) * attackSpeed;
            float fractionOfJourney = distCovered / attackLength;
            spaceshipSprite.position = Vector3.Lerp(attackSource, attackDestination.position, fractionOfJourney);

            float distance = Vector3.Distance(attackDestination.position, spaceshipSprite.position);
            if (distance < 0.1f) {
                GameObject go = Instantiate(explosionPrefab, spaceshipSprite);
                state = State.Destroy;
                StartCoroutine(SelfDestruct());
            }
        }
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
