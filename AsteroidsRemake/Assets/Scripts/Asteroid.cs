using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject explosion;
    public GameObject asteroid;
    public float speed = 2f;
    public bool smallAsteroid;
    public int points = 25;

    private float verticalHalfSize;
    private float horizontalHalfSize;
    Vector2 direction;
    SceneManager sceneManager;

    public delegate void OnDestroyed();
    public static event OnDestroyed onSmallAsteroidDestroyed;

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
            GameObject effect = Instantiate(explosion, transform.position, transform.rotation);
            effect.transform.localScale = transform.localScale * 5;
            if (!smallAsteroid)
            {
                Instantiate(asteroid, transform.position, transform.rotation);
                Instantiate(asteroid, transform.position, transform.rotation);
            }
            else
            {
                if(onSmallAsteroidDestroyed != null)
                {
                    onSmallAsteroidDestroyed();
                }
            }
            Destroy(collision.gameObject);
            sceneManager.IncreaseScore(points);
            Destroy(gameObject);
        }
    }
}