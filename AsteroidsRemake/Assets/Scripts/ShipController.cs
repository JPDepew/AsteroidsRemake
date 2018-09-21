using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject gunPosition;
    public GameObject bullet;
    public GameObject explosion;

    public float maxSpeed = 2;
    public float acceleration = 0.1f;
    public float lookSpeed = 1;
    public float maxLookSpeed = 5;

    private Vector2 direction;
    private float rotateAmount = 0;

    float verticalHalfSize;
    float horizontalHalfSize;

    private void Start()
    {
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    void Update()
    {
        GetInput();
        HandleWrapping();

        transform.position = transform.position + (Vector3)direction * Time.deltaTime;
        transform.Rotate(0, 0, rotateAmount);
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += (Vector2)(transform.up * acceleration);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (rotateAmount < maxLookSpeed)
                rotateAmount += lookSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (rotateAmount > -maxLookSpeed)
                rotateAmount += -lookSpeed * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            rotateAmount = Mathf.Lerp(rotateAmount, 0, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, gunPosition.transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// If the ship goes off the screen, it is wrapped around to the other side
    /// </summary>
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
        if (collision.tag == "Asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            SceneManager.instance.DestroyPlayer();
        }
    }
}
