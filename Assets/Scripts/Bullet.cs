﻿using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 1f;
    public float timeTillDeath = 1f;

    private float targetTime = 0;

    private void Start()
    {
        targetTime = Time.time + timeTillDeath;
    }

    void Update () {
        transform.Translate(Vector3.up * speed);
        if(Time.time > targetTime)
        {
            Destroy(gameObject);
        }
	}
}
