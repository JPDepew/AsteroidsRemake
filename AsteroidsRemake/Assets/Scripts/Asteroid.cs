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

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private float verticalHalfSize;
    private float horizontalHalfSize;
    Vector2 direction;
    SceneManager sceneManager;
    private bool shouldDestroyAsteroid = false;
    private float targetTime = 0;

    public delegate void OnDestroyed();
    public static event OnDestroyed onSmallAsteroidDestroyed;

    void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.insideUnitCircle.normalized;
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    void Update()
    {
        HandleDestroyingAsteroid();
        transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
        HandleWrapping();
    }

    private void HandleDestroyingAsteroid()
    {
        if (shouldDestroyAsteroid)
        {
            if (Time.time > targetTime)
            {
                Destroy(gameObject);
            }
        }
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
                if (onSmallAsteroidDestroyed != null)
                {
                    onSmallAsteroidDestroyed();
                }
            }
            Destroy(collision.gameObject);
            sceneManager.IncreaseScore(points);
            audioSource.Play();
            spriteRenderer.color = new Color(0, 0, 0, 0);
            GetComponent<Collider2D>().enabled = false;
            shouldDestroyAsteroid = true;
            targetTime = Time.time + 1f;
        }
    }
}