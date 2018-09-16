using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 2f;
    Vector2 direction;

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
    }
}