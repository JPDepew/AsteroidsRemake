using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject explosion;
    public GameObject asteroid;
    public float speed = 2f;
    public bool lastAsteroid;

    private float verticalHalfSize;
    private float horizontalHalfSize;
    Vector2 direction;
    SceneManager sceneManager;

    void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        direction = Random.insideUnitCircle.normalized;
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
        HandleWrapping();
    }

    private void HandleWrapping()
    {
        if (transform.position.y > verticalHalfSize)
        {
            transform.position = new Vector2(transform.position.x, -verticalHalfSize);
        }
        if (transform.position.y < -verticalHalfSize)
        {
            transform.position = new Vector2(transform.position.x, verticalHalfSize);
        }
        if (transform.position.x > horizontalHalfSize)
        {
            transform.position = new Vector2(-horizontalHalfSize, transform.position.y);
        }
        if (transform.position.x < -horizontalHalfSize)
        {
            transform.position = new Vector2(horizontalHalfSize, transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            if (!lastAsteroid)
            {
                Instantiate(asteroid, transform.position, transform.rotation);
                Instantiate(asteroid, transform.position, transform.rotation);
            }
            Destroy(collision.gameObject);
            sceneManager.IncreaseScore();
            Destroy(gameObject);
        }
    }
}